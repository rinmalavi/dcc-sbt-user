package namespace.myModule.repositories;

public class ARepository extends com.dslplatform.client.ClientPersistableRepository<namespace.myModule.A> {
	public ARepository(final com.dslplatform.patterns.ServiceLocator locator) {
		super(namespace.myModule.A.class, locator);
	}
}