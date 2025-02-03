
### Database migrations
If you made any changes to the database structure navigate to the `TRAFO/TRAFO.IO` folder. This is the folder in which the project containing the database logic is. Once you're there run:
`dotnet ef migrations add NAME_OF_YOUR_MIGRATION`
This will generate the new migration file. To get the database up to date run:
`dotnet ef database update`