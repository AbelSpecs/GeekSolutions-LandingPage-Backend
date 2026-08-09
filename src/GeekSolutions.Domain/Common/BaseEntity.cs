namespace GeekSolutions.Domain.Common
{
	public abstract class BaseEntity
	{
		public int Id { get; protected set; } = 0;
		public DateTime CreatedAt { get; protected set; } = DateTime.UtcNow;
		public DateTime? UpdatedAt { get; protected set; }

		public void Update()
		{
			UpdatedAt = DateTime.UtcNow;
		}
	}
}