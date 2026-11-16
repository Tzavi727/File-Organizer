namespace FileOrganizer.Interfaces;

public interface ISorterService
{
    int ExecuteOrganization(string path);
    int ExecuteOrganizationByExtension(string path, string extension, string destination);
}