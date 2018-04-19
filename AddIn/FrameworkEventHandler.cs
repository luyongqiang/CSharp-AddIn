namespace Syncfusion.Addin
{
    /// <summary>
    /// Prototype of the method to be implemented to receive framework events.
    /// </summary>
    /// <param name="sender">The event sender</param>
    /// <param name="e">The event argurment <see cref="FrameworkEventArgs"/></param>
    public delegate void FrameworkEventHandler(object sender, FrameworkEventArgs e);
}