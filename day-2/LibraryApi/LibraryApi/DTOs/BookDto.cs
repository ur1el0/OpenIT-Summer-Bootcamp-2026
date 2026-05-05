namespace LibraryApi.DTOs;


public class BookCreateDto {
    public string Title  { get; set; } = "";
    public string Author { get; set; } = "";
    public string Genre  { get; set; } = "";
}

// GET response — what SERVER returns (includes Id)
public class BookResponseDto {
    public int    Id     { get; set; }
    public string Title  { get; set; } = "";
    public bool Available { get; set; }
    // Internal fields like audit timestamps NOT exposed
}
