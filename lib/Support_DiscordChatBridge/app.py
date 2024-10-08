from flask import Flask, request
from threading import Thread
from werkzeug import urls as werkurls
import discord
import asyncio
import socket
import re
import demoji

intents = discord.Intents.all()
client = discord.Client(intents=intents)
app = Flask(__name__)
client_loop = None
server2blkey = ''
bl2serverkey = ''
discordkey = ''
debug = False

emoji_list = []
emoji_list_init = False

channelID = 784285464340987924
privateChannelID = 754466520439980056
currentChannel = channelID

async def client_start():
	await client.start(discordkey)


def run_forever(loop):
	loop.run_forever()


def init():
	global client_loop

	loop = asyncio.get_event_loop()
	loop.create_task(client_start())
	client_loop = loop

	Thread(target=run_forever, args=(loop,)).start()


async def forward_message(message):
	s = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
	s.connect(('localhost', 28008))
	if debug:
		s.send((message + '\n').encode('cp1252', errors='replace'))
	else:
		s.send((server2blkey + '\t' + message + '\n').encode('cp1252', errors='replace'))
	s.close()
	return


def filter_string(string):
	string = re.sub(r'<font.+?>', '', string)
	string = re.sub(r'<color.+?>', '', string)
	string = re.sub(r'<div.+?>', '', string)
	string = re.sub(r'<a.+?>', '', string)
	string = re.sub(r'<bitmap.+?>', '', string)
	string = re.sub(r'<just.+?>', '', string)
	string = re.sub(r'<tab.+?>', '', string)
	string = re.sub(r'<lmargin.+?>', '', string)
	string = re.sub(r'<rmargin.+?>', '', string)
	string = re.sub(r'<clip.+?>', '', string)
	string = re.sub(r'<shadow.+?>', '', string)
	string = re.sub(r'<shadowcolor.+?>', '', string)

	string = re.sub(r'<spush>', '', string)
	string = re.sub(r'<spop>', '', string)
	string = re.sub(r'<sbreak>', '', string)
	string = re.sub(r'<br>', '', string)

	return string


def create_emoji_list():
	global emoji_list, emoji_list_init, client

	if emoji_list_init:
		return emoji_list

	for emoji in client.emojis:
		emoji_list.append((emoji.name, emoji.id))

	emoji_list_init = True
	return emoji_list


#sending custom emojis from ingame to discord
def replace_transmit_emojis(string, emoji_list):
	for emoji_name, emoji_id  in emoji_list:
		string = re.sub(
			':' + emoji_name + ':', 
			'<:' + emoji_name + ':' + str(emoji_id) + '>', 
			string)
	return string


#sending discord custom emojis to ingame
def replace_forward_emojis(string, emoji_list):
	for emoji_name, emoji_id in emoji_list:
		string = re.sub(
			'<:' + emoji_name + ':' + str(emoji_id) + '>',
			':' + emoji_name + ':',
			string)
	return string


@client.event
async def on_ready():
	print('Logged in as {0.user}'.format(client))


@client.event
async def on_message(message):
	if message.author == client.user:
		return

	if message.channel.id == currentChannel:
		if message.content == '!players':
			await forward_message('playerlist')
			return

		name = message.author.nick if message.author.nick else message.author.name
		attachments = message.attachments
		messageString = ''

		emoji_list = create_emoji_list()

		# label messages if they have attachments
		for a in attachments:
			messageString = messageString + '[<a:' + a.proxy_url + ">" + a.filename + '</a>] '
		# base message
		messageString = name + '\t' + messageString + message.clean_content
		# convert emojis into {plaintext}
		messageString = demoji.replace_with_desc(messageString)
		# replace server custom emoji with short form
		messageString = replace_forward_emojis(messageString, emoji_list)


		#remove TML
		newString = filter_string(messageString)
		while messageString != newString:
			messageString = newString
			newString = filter_string(messageString)

		# replace url's with <a:url>url</a>
		urls = re.findall(r'https:\/\/\S+', messageString)
		for url in urls:
			messageString = messageString.replace(url, '<a:' + url + '>' + url[8:])

		urls = re.findall(r'http:\/\/\S+', messageString)
		for url in urls:
			messageString = messageString.replace(url, '<a:' + url + '>' + url[7:])

		messageString = messageString.replace('\n', ' ')

		#strip non-cp1252 characters
		# encoded = messageString.encode('cp1252', errors='replace')
		# print("Encoded to cp1252:")
		# print(encoded)
		# messageString = messageString.encode('cp1252', errors='replace')#.decode().encode('utf-8').decode()

		print(messageString)
		sendCount = 0
		while len(messageString) > 160 and sendCount < 1:
			await forward_message(messageString[:160])
			messageString = ' >\t' + messageString[160:]
			print('Cut message: ' + messageString)
			sendCount += 1
		if len(messageString) > 160:
			await forward_message(messageString[:160] + ' [message truncated]')
		else:
			await forward_message(messageString)




async def sendMessage(message):
	channel = client.get_channel(currentChannel)
	if channel:
		await channel.send(message)


async def postPlayerList(playerlist):
	channel = client.get_channel(currentChannel)
	embed = discord.Embed(
		title = ':joystick: Players',
		colour = discord.Color.blue(),
		type = 'rich'
	)

	header = ''.ljust(6) + 'Name'.ljust(25) + 'Score'.ljust(8) + 'BLID'
	print(header)
	message = '```' + header + '\n'
	for blid, name in playerlist.items():
		admin, name, score = name.split('\t')
		admin = admin.ljust(4)
		name = name.ljust(25)
		score = score.ljust(8)
		message = message + f'{admin}{name}{score}{blid}\n'
	message = message + '```'

	embed.add_field(name='Current player list', value=message)
	if channel:
		await channel.send(embed=embed)


async def purgeMessages():
	channel = client.get_channel(currentChannel)
	if channel:
		await channel.purge(limit=20000)


@app.route('/rcvmsg', methods=['POST'])
def transmit_message():
	if request.method == 'POST':
		request.charset = 'cp1252'
		form = werkurls.url_decode(request.get_data(), charset='cp1252').to_dict()
		if form['verifykey'] != bl2serverkey:
			return;

		emoji_list = create_emoji_list()

		if form['type'] == 'raw':
			message = form['message']
		elif form['type'] == 'connection':
			message = '**{} ({}) {}**'.format(form['author'], form['bl_id'], form['message'])
		else:
			message = '**{}** ({}): {}'.format(form['author'], form['bl_id'], form['message'])

		# remove @mentions
		message = discord.utils.escape_mentions(message)

		message = message.replace('\n', '')
		message = message.replace('\r', '')
		message = message.replace(':^)', '`:^)`')

		# replace incoming short custom emoji names with server custom emojis
		message = replace_transmit_emojis(message, emoji_list)

		urls = re.findall(r'https:\/\/\S+', message)
		if urls:
			for url in urls:
				message = message.replace(url, '<' + url + '>')

		urls = re.findall(r'http:\/\/\S+', message)
		if urls:
			for url in urls:
				message = message.replace(url, '<' + url + '>')

		print(message)
		send_fut = asyncio.run_coroutine_threadsafe(sendMessage(message), client_loop)
		return 'Success'


@app.route('/purge', methods=['POST'])
def purge_messages():
	if request.method == 'POST':
		form = request.form.to_dict()
		if form['verifykey'] != bl2serverkey:
			return;
		send_fut = asyncio.run_coroutine_threadsafe(purgeMessages(), client_loop)
		return 'Success'

@app.route('/toggleChannel', methods=['POST'])
def toggle_channel():
	if request.method == 'POST':
		global currentChannel
		form = request.form.to_dict()
		if form['verifykey'] != bl2serverkey:
			return;
		if currentChannel == channelID:
			currentChannel = privateChannelID
		else:
			currentChannel = channelID
		return 'Success ' + str(currentChannel);


@app.route('/sendplayerlist', methods=['POST'])
def player_list():
	if request.method == 'POST':
		form = request.form.to_dict()
		if form['verifykey'] != bl2serverkey:
			return

		form.pop('verifykey', None)

		print(form)
		send_fut = asyncio.run_coroutine_threadsafe(postPlayerList(form), client_loop)
		return 'Success'




init()
app.run(port=28010, debug=False, threaded=True)
