SET Sz="%ProgramFiles%\7-Zip\7z.exe"
IF NOT EXIST %Sz% GOTO :eof

SET outdir=X:\Programs\www\Public\s\ahk
IF NOT EXIST "%outdir%" GOTO :eof

SET out=%outdir%\binaries.zip
IF EXIST "%out%" DEL /F /Q "%out%"
%Sz% a -tzip "%out%" -mx=9 -r "%2*" -x!*.vshost.*

SET sh=robokins-launch.sh
SET out=%outdir%\%sh%
IF EXIST "%out%" DEL /F /Q "%out%"
COPY "%3%sh%" "%out%"
