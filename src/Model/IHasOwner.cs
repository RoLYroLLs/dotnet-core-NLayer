namespace NLayer {
	/// <summary>
	/// This interface is used to mark the owner of an object.
	/// </summary>
	public interface IHasOwner<T> {
		/// <summary>
		/// The Owner instance this object belongs to.
		/// </summary>
		T Owner { get; set; }
	}
}
