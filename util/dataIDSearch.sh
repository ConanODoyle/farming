#! /bin/bash

# Parse options
blid="-1"
item=""
croptrak=""

while [[ $# -gt 0 ]]; do
  case "$1" in
    --blid=*)
      blid="${1#*=}"
      shift
      ;;
    --item=*)
      item="${1#*=}"
      shift
      ;;
    --croptrak=*)
      croptrak="${1#*=}"
      shift
      ;;
    --help)
      echo "Usage: $0 [--file filename] [--verbose]"
      exit 0
      ;;
    -*)
      echo "Unknown option: $1" >&2
      exit 1
      ;;
    *)
      # Positional argument
      break
      ;;
  esac
done

mapfile -t files < <(egrep -l "lastUserBLID = \"$blid\"" DataIDs/*)

if [[ -n "$item" && ${#files[@]} -gt 0 ]]; then
    mapfile -t files < <(printf "%s\n" "${files[@]}" | xargs egrep -l "$item")
fi

if [[ -n "$croptrak" && ${#files[@]} -gt 0 ]]; then
    mapfile -t files < <(printf "%s\n" "${files[@]}" | xargs egrep -l "statTrakType = \"$croptrak\"")
fi

if [[ ${#files[@]} -eq 0 ]]; then
	echo "No files found"
else
	printf "%s\n" "${files[@]}" | xargs -n1 basename | paste -sd, -
fi