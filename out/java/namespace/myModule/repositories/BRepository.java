package namespace.myModule.repositories;

public class BRepository extends com.dslplatform.client.ClientPersistableRepository<namespace.myModule.B> {
	public BRepository(final com.dslplatform.patterns.ServiceLocator locator) {
		super(namespace.myModule.B.class, locator);
	}
}