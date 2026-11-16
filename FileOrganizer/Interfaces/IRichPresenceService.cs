using System.Threading.Tasks;

namespace FileOrganizer.Interfaces;

public interface IRichPresenceService
{
    void InitializeRpc();
    void DisposeClient();
    void SetIdlePresence();
    Task SetActionPresence(string details, string state);
    void UpdateStatus(string details, string state);
}