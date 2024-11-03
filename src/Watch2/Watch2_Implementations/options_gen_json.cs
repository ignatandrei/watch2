
namespace NS_GeneratedJson_options_gen_json;
partial class options_gen_json: Ioptions_gen_json
{
    public static readonly options_gen_json Empty = new options_gen_json();

    public IEnumerable<System.ComponentModel.DataAnnotations.ValidationResult> Validate(ValidationContext validationContext)
    {
        if(Version == null)
            yield return new System.ComponentModel.DataAnnotations.ValidationResult("Version is required", new[] { "Version" });
        if(Version<1)
            yield return new System.ComponentModel.DataAnnotations.ValidationResult("Version must be greater than 0", new[] { "Version" });
    }
}

