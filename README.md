# Summary
Current version (0.1) is only meant for internal use. Documentation and instruction on how to use the program will be done in 0.3, but feel free to look around!

# Personal note
This is a hobby project. I know some of the code structures are not optimal, could be more layered or stricter. The point of this project is a pragmatic approach to this problem, a fun project for me to work on and a practical solution to a real life problem people have.

# Developer notes
### Database migrations
If you made any changes to the database structure navigate to the `TRAFO/TRAFO.IO` folder. This is the folder in which the project containing the database logic is. Once you're there run:
`dotnet ef migrations add NAME_OF_YOUR_MIGRATION`
This will generate the new migration file. To get the database up to date run:
`dotnet ef database update`
