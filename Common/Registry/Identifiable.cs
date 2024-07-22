namespace Twita.Common.Registry
{

	public interface Identifiable
	{

		public Identity Registry { get; set; }

		public virtual void OnRegistry() { }

	}

}