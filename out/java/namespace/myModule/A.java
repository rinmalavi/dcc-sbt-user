package namespace.myModule;

public class A   implements java.io.Serializable, com.dslplatform.patterns.AggregateRoot {

	public A() {

		_serviceLocator = com.dslplatform.client.Bootstrap.getLocator();
		_domainProxy = _serviceLocator.resolve(com.dslplatform.client.DomainProxy.class);
		_crudProxy = _serviceLocator.resolve(com.dslplatform.client.CrudProxy.class);
		this.ID = 0;
	}

	private transient final com.dslplatform.patterns.ServiceLocator _serviceLocator;
	private transient final com.dslplatform.client.DomainProxy _domainProxy;
	private transient final com.dslplatform.client.CrudProxy _crudProxy;

	private String URI;

	@com.fasterxml.jackson.annotation.JsonProperty("URI")
	public String getURI()  {

		return this.URI;
	}

	@Override
	public int hashCode() {
		return URI != null ? URI.hashCode() : super.hashCode();
	}

	@Override
	public boolean equals(final Object obj) {
		if (this == obj)
			return true;
		if (obj == null)
			return false;

		if (getClass() != obj.getClass())
			return false;
		final A other = (A) obj;

		return URI != null && URI.equals(other.URI);
	}

	@Override
	public String toString() {
		return URI != null ? "A(" + URI + ')' : "new A(" + super.hashCode() + ')';
	}

	private static final long serialVersionUID = 0x0097000a;

	@com.fasterxml.jackson.annotation.JsonCreator private A(
			@com.fasterxml.jackson.annotation.JacksonInject("_serviceLocator") final com.dslplatform.patterns.ServiceLocator _serviceLocator,
			@com.fasterxml.jackson.annotation.JsonProperty("URI") final String URI ,
			@com.fasterxml.jackson.annotation.JsonProperty("ID") final int ID) {
		this._serviceLocator = _serviceLocator;
		this._domainProxy = _serviceLocator.resolve(com.dslplatform.client.DomainProxy.class);
		this._crudProxy = _serviceLocator.resolve(com.dslplatform.client.CrudProxy.class);
		this.URI = URI;
		this.ID = ID;
	}

	private int ID;

	@com.fasterxml.jackson.annotation.JsonProperty("ID")
	@com.fasterxml.jackson.annotation.JsonInclude(com.fasterxml.jackson.annotation.JsonInclude.Include.NON_EMPTY)
	public int getID()  {

		return ID;
	}

	private A setID(final int value) {

		this.ID = value;

		return this;
	}

	public static A find(final String uri) throws java.io.IOException {
		return find(uri, com.dslplatform.client.Bootstrap.getLocator());
	}
	public static A find(final String uri, final com.dslplatform.patterns.ServiceLocator locator) throws java.io.IOException {
		try {
			return (locator != null ? locator : com.dslplatform.client.Bootstrap.getLocator()).resolve(com.dslplatform.client.CrudProxy.class).read(A.class, uri).get();
		} catch (final InterruptedException e) {
			throw new java.io.IOException(e);
		} catch (final java.util.concurrent.ExecutionException e) {
			throw new java.io.IOException(e);
		}
	}
	public static java.util.List<A> find(final Iterable<String> uris) throws java.io.IOException {
		return find(uris, com.dslplatform.client.Bootstrap.getLocator());
	}
	public static java.util.List<A> find(final Iterable<String> uris, final com.dslplatform.patterns.ServiceLocator locator) throws java.io.IOException {
		try {
			return (locator != null ? locator : com.dslplatform.client.Bootstrap.getLocator()).resolve(com.dslplatform.client.DomainProxy.class).find(A.class, uris).get();
		} catch (final InterruptedException e) {
			throw new java.io.IOException(e);
		} catch (final java.util.concurrent.ExecutionException e) {
			throw new java.io.IOException(e);
		}
	}
	public static java.util.List<A> findAll() throws java.io.IOException {
		return findAll(null, null, com.dslplatform.client.Bootstrap.getLocator());
	}
	public static java.util.List<A> findAll(final com.dslplatform.patterns.ServiceLocator locator) throws java.io.IOException {
		return findAll(null, null, locator);
	}
	public static java.util.List<A> findAll(final Integer limit, final Integer offset) throws java.io.IOException {
		return findAll(limit, offset, com.dslplatform.client.Bootstrap.getLocator());
	}
	public static java.util.List<A> findAll(final Integer limit, final Integer offset, final com.dslplatform.patterns.ServiceLocator locator) throws java.io.IOException {
		try {
			return (locator != null ? locator : com.dslplatform.client.Bootstrap.getLocator()).resolve(com.dslplatform.client.DomainProxy.class).findAll(A.class, limit, offset, null).get();
		} catch (final InterruptedException e) {
			throw new java.io.IOException(e);
		} catch (final java.util.concurrent.ExecutionException e) {
			throw new java.io.IOException(e);
		}
	}
	public static java.util.List<A> search(final com.dslplatform.patterns.Specification<A> specification) throws java.io.IOException {
		return search(specification, null, null, com.dslplatform.client.Bootstrap.getLocator());
	}
	public static java.util.List<A> search(final com.dslplatform.patterns.Specification<A> specification, final com.dslplatform.patterns.ServiceLocator locator) throws java.io.IOException {
		return search(specification, null, null, locator);
	}
	public static java.util.List<A> search(final com.dslplatform.patterns.Specification<A> specification, final Integer limit, final Integer offset) throws java.io.IOException {
		return search(specification, limit, offset, com.dslplatform.client.Bootstrap.getLocator());
	}
	public static java.util.List<A> search(final com.dslplatform.patterns.Specification<A> specification, final Integer limit, final Integer offset, final com.dslplatform.patterns.ServiceLocator locator) throws java.io.IOException {
		try {
			return (locator != null ? locator : com.dslplatform.client.Bootstrap.getLocator()).resolve(com.dslplatform.client.DomainProxy.class).search(specification, limit, offset, null).get();
		} catch (final InterruptedException e) {
			throw new java.io.IOException(e);
		} catch (final java.util.concurrent.ExecutionException e) {
			throw new java.io.IOException(e);
		}
	}
	public static long count() throws java.io.IOException {
		return count(com.dslplatform.client.Bootstrap.getLocator());
	}
	public static long count(final com.dslplatform.patterns.ServiceLocator locator) throws java.io.IOException {
		try {
			return (locator != null ? locator : com.dslplatform.client.Bootstrap.getLocator()).resolve(com.dslplatform.client.DomainProxy.class).count(A.class).get().longValue();
		} catch (final InterruptedException e) {
			throw new java.io.IOException(e);
		} catch (final java.util.concurrent.ExecutionException e) {
			throw new java.io.IOException(e);
		}
	}
	public static long count(final com.dslplatform.patterns.Specification<A> specification) throws java.io.IOException {
		return count(specification, com.dslplatform.client.Bootstrap.getLocator());
	}
	public static long count(final com.dslplatform.patterns.Specification<A> specification, final com.dslplatform.patterns.ServiceLocator locator) throws java.io.IOException {
		try {
			return (locator != null ? locator : com.dslplatform.client.Bootstrap.getLocator()).resolve(com.dslplatform.client.DomainProxy.class).count(specification).get().longValue();
		} catch (final InterruptedException e) {
			throw new java.io.IOException(e);
		} catch (final java.util.concurrent.ExecutionException e) {
			throw new java.io.IOException(e);
		}
	}
	private void updateWithAnother(final namespace.myModule.A result) {
		this.URI = result.URI;

		this.ID = result.ID;
	}
	public A persist() throws java.io.IOException {
		final A result;
		try {
			result = this.URI == null ? _crudProxy.create(this).get() : _crudProxy.update(this).get();
		} catch (final InterruptedException e) {
			throw new java.io.IOException(e);
		} catch (final java.util.concurrent.ExecutionException e) {
			throw new java.io.IOException(e);
		}
		this.updateWithAnother(result);
		return this;
	}
	public A delete() throws java.io.IOException {
		try {
			return _crudProxy.delete(A.class, URI).get();
		} catch (final InterruptedException e) {
			throw new java.io.IOException(e);
		} catch (final java.util.concurrent.ExecutionException e) {
			throw new java.io.IOException(e);
		}
	}
}
