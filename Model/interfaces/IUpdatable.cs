
namespace Interfaces
{
    // Interface definition for objects that can be updated over time
    internal interface IUpdatable
    {
        // Method to update the object based on the elapsed time since the last update
        void Update(TimeSpan deltaTime);
    }
}
