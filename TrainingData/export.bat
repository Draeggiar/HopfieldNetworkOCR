@ECHO OFF
set f=arial.ttf
set ps=275
set bg=white
set ext=tiff
set s=250x300
set alpha=A B C D E F G H I J K L M N O P Q R S T U V W Z Y Z

echo Exporting characters ...
For %%X in (%alpha%) do (
	convert -font %f% -pointsize %ps% -size %s% -background %bg% label:%%X %%X.%ext%
	convert %%X.%ext% -resize 10x12^ -trim +repage %%X.%ext%
	convert %%X.%ext% -resize 10x12! -colorspace gray +dither -colors 2 -normalize %%X.%ext%
)
pause
exit