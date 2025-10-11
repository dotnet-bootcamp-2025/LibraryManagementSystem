namespace LibraryApp.Domain;

public class Book : LibraryItem
{
    //constructor chaining es similar al constructor de java con el this
    //Key benefits
    // 1. DRY (don't repeat yourself) no repitas codigo con el que ya cuentas
    // 1.	DRY Principle: The validation logic (string.IsNullOrWhiteSpace, pages < 0) exists in only one place
    // 2.	Maintenance: Change validation rules once, affects all constructors
    // 3.	Consistency: All constructors follow the same validation path
    public Book(int id, string title, string author, int pages, bool isBorrowed, bool active) : base(id, title, isBorrowed, active)
    {
        Author = string.IsNullOrEmpty(author) ? "Unknown" : author;
        Pages = pages<0?0: pages; //el ? es un if
        IsBorrowed = isBorrowed;
        Active = active;
    }

    public string Author { get;  }
    public int Pages { get; }
    
    //constructor generated when we put LibraryItem
   
    public override string GetInfo() =>
        $"Book[Id={Id}, Title={Title}, Author={Author}, Pages={Pages}, IsBorrowed={IsBorrowed}, Active={Active}]";
    //unimplemented members added nota: rider no ofrece actualizacion de override, ver este como referencia
}