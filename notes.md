# Helpful Notes about development

## Linux

- To get past the libtinfo.so.5 error I had to: `sudo apt install libncurses5`

## Webgl

- To fix blue screen: Try to go to Project Settings -> Player -> Settings for WebGL -> Other Settings -> Color Space from gamma to linear -> Unmark Auto Graphics API -> Remove WebGL 1.0 from Graphics APIs list and try build again
	+ Found at: https://forum.unity.com/threads/webgl-build-gives-a-blue-screen.914222/
