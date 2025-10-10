namespace LibraryApp.Api.Records
{
    public class LibraryController
    {
        //public class BookDto() //mover a otra carpeta dentro de libraryApp.Api estos son los Dto
        //{
        //    public string Title { get; set; }
        //    public string Author { get; set; }
        //    public int Pages { get; set; }
        //}

        // records se hace asi:
        public record BookRecord (string Title, string Author, int Pages) { }



    }
}
