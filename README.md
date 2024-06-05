# Console App Rhythm Game
A game made in just over a week that I made to learn C#. <br />
Its just a simple 4-9k rhythm game where you hit notes in time with a song, <br />
but its made entirely inside of console app, and uses text for its sprites/graphics. <br />
![](https://github.com/z011747/console-app-rhythm-game/blob/master/readme/gameplay.gif)
 <br />
It supports custom songs and loading file formats from a couple different rhythm games (FNF .json charts (pre 0.3) and StepMania .sm files). <br />
The game also has its own file format for charts but has no editor for it to be easily useable. <br />
![](https://github.com/z011747/console-app-rhythm-game/blob/master/readme/options.png)
<br />
There are multiple options that can be changed in the menu, such as the scroll speed of notes and the scroll direction. <br />
The game also supports custom noteskins that can be setup with different colors and sprites (using text in txt files). <br />
![](https://github.com/z011747/console-app-rhythm-game/blob/master/readme/noteskins.png)
## Development of the game
I first had the idea of creating a simple game in console app while talking with a friend, <br />
and decided it would be a good way to start learning C# for my University Course as I was starting a Networking unit that uses C# and .NET. <br />
I got the idea to do a rhythm game mainly because I play them often and enjoy them a lot, <br /> but also because I have experience modding FNF (an open source rhythm game), <br />
so I already had a basic understanding of how to create a rhythm game, and how to setup notes and how chart formats work, etc. <br />
<br />
One of the main issues I had to figure out when making this was how to have to console clear and <br /> draw new text each frame without stuttering or lag (since Console.clear() is slow) <br />
I managed to do this by typing empty spaces over the screen before each draw and then drawing to the screen. <br /> 
There were other issues with drawing such as making multiline text objects match the correct lines and clip to the top and left side of the screen, etc. <br /> 
<br /> 
Once I had the main framework with text objects setup I could start actually making the game, <br />
I first made the game display notes moving with a time variable that was increasing with deltatime, <br /> 
and a basic chart format/loader and a converter so I could test it. <br /> 
After that i added things like long note rendering (did by seeing how far away the end of the long note was and looping a draw for each space in between), 
and implementing an input system and being able to hit notes, the input needed to only detect the first press and ignore it being held, and adding the audio obviously. <br />
<br />
At this point that the game was technically playable and was just lacking extra features, <br /> 
which were added over the next few days, things like the HUD, Color on the notes, custom song support, menus, etc. <br />
There were even some extra features I added such as Lua scripting (using a library for) which means you could make modcharts for songs,
and do things like moving arrows, moving the screen, etc. <br /> 
<br /> 
Overall, this game was pretty fun to work on and has helped me learn and get used to working with C#.
<br /> 
<br /> 
