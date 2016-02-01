##Duck: The Rip Offering
This game is a turn based RPG, kind of like Pokemon, but in no way is it associated with it or Nintendo.

######The Game itself
The idea of the game is, just as in Pokemon, to capture monsters, train them, and ultimately defeat the strongest guy in the region.

######Installing
- To install the game you should first go and download any program which can host a database. (WAMP, XAMPP, etc)
- When the installation is complete open up either the XAMPP Control Panel and start both Apache and MySql, or right click on the WAMP icon and enable all services.
- Open your preffered browser and go to the following address: [localhost/phpmyadmin](localhost/phpmyadmin)
- Create a new table called `ripoff`, you can give this a custom name, but you'll have to alter a few more settings
- Click on the `Import` tab, click the `Browse...` button and navigate to the .sql file which came with the zip you can find at releases.
- Download and install the [OpenAL Windows Installer](https://www.openal.org/downloads/)
- Now you can go and launch the game. Make sure your program is running and MySQL is running.
- Since this is the first time you're launching the game you should go and create a new account.
- This is done by simply entering a username and password in the fields, clicking on register, choosing a gender and clicking on the Yes button when prompted.
- You have now succesfully installed the game. To continue with your progress just login with the same username and password.

######Building
Just download the entire project and start it up with Visual Studio 2015, older versions won't work.
Set `SandBox` as your startup project and build the solution.
It should work just fine after that
