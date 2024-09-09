#!/bin/sh
echo -ne '\033c\033]0;[Insert Name Here]\a'
base_path="$(dirname "$(realpath "$0")")"
"$base_path/[Insert Name Here].x86_64" "$@"
