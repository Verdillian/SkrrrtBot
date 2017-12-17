# KnightBot

This discord bot is going to include some cool things.


Things that are currently implemented:
  - Music Bot
    - !play <youtubeURL> - Plays a song from youtube, if it is not a direct link to the song it will break (working on finding a solution.)
    - !stop - Stops the song and removes the bot from the current voice channel.
  - Fighting Game
    - !fight <@(usersname)> - Allows you to have a sword fight with a user.
    - !giveup - Gives up the fight...
    - !slash - Slash your oponent with a sword!
  - Connection To Database (allows you to be able to give people tokens / eventually will gain tokens from talking in the discord.)
    - Current token system commands:
      - !status - Tells you how many tokens you have.
      - !status <@(username)> - Tells you how many tokens another user has. (slightly buggy but still works)
  - Clear Chat
    - !clear, !c - Clears the messages in  a text channel up to 100
  - Auction System (not real auction for the tokens, just for fun.)
    - Auction Commands (Kind of buggy and doesnt always work...):
      - !auction <startingBid> <amountOfItem> <item> - Starts a auction. Ex: !auction 5 1 :skull:
      - !bid <bidAmount>
      - !auctionend - Ends a auction if one is going.
      - !auctioncheck - Checks to see if a auction is currently going.
  - DM creator of bot
    - !dm <msg> - DM's the creater of the bot what ever the message is.
  - Set the bots current game
    - !setgame <text> - Sets the bots game to what ever the text is.
  - Random eightball
    - !8ball <text> - The bot gives you a random message telling you what it thinks will happen according to what you put in.
  - Administrative Commands:
    - !kick <user> <reason> - Kicks a user from the discord server.
    - !ban <user> <reason> - Bans a user from the discord server.
    - !addrole <user> <role> - Adds a role to a user.
    - !remorole <user> <role> - Deletes a role from a user.
    
Things that are planned to be implemented:
  - Volume command for music bot.
  - Hangman
  - NSFW Bot / meme bot (depends which is more fun to do)
  - TicTacToe
  - Possible Cards Against Humanity Like Game.
