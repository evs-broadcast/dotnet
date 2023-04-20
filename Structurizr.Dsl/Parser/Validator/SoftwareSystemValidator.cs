namespace Structurizr.Dsl.Parser.Validator
{
    public static class SoftwareSystemValidator
    {
        const string prefix = "ss_";
        public static void Validate(SoftwareSystem softwareSystem)
        {
            if (!softwareSystem.Id.StartsWith(prefix))
                throw new Exception($"SoftwareSystem prefix MUST be {prefix} ({softwareSystem.Id})");
        }
    }
}