namespace Common.DB.Model
{
    public class BaseEntity
    {
        // add basic info to all the database entities
        public DateTime Created { get; set; }
        public DateTime Modified { get; set; }

        public BaseEntity()
        {
            Created = DateTime.UtcNow;
            Modified = DateTime.UtcNow;
        }
    }
}
