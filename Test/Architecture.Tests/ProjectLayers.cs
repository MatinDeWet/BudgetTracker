using System.Reflection;
using API;
using Application;
using Domain;
using Persistence;

namespace Architecture.Tests;

public abstract class ProjectLayers
{
    protected static readonly Assembly DomainAssembly = typeof(IDomainPointer).Assembly;
    protected static readonly Assembly ApplicationAssembly = typeof(IApplicationPointer).Assembly;
    protected static readonly Assembly InfrastructureAssembly = typeof(IPersistencePointer).Assembly;
    protected static readonly Assembly PresentationAssembly = typeof(IApiPointer).Assembly;
}
