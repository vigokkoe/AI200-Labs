namespace SearchBootstrapper.Provisioning;

public interface ISearchProvisioner
{
    Task ProvisionAsync(CancellationToken cancellationToken = default);
}