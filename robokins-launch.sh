#!/bin/sh

name=robokins
src=.src

mono=mono
exe=$name.exe
zip=binaries.zip
pid=$name.pid
log=output.log
err=error.log

if [ "$1" != "-c" ]; then
	if [ ! -e "$src" ]; then
		echo "Source location file not found."
		exit
	fi
	if [ -e "$zip" ]; then rm "$zip"; fi
	wget $(cat "$src")$zip -O "$zip"
	if [ -n $(which "7za" 2>/dev/null) ]; then
		if [ -n $(which "unzip" 2>/dev/null) ]; then
			unzip -o "$zip"
		else
			echo "ZIP extract utility not found."
			exit
		fi
	else
		7za x -aoa "$zip"
	fi
	rm "$zip"
fi

if [ -e "$pid" ]; then
	run=$(cat "$pid")
	if [ -n "$run" ]; then
		if ps x | grep "$run" 2>/dev/null > /dev/null; then kill "$run" 2>/dev/null; fi
	fi
fi

"$mono" "$exe" >>"$log" 2>>"$err" &
