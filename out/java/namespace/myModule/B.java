package namespace.myModule;

public class B   implements java.io.Serializable, com.dslplatform.patterns.AggregateRoot {

	public B() {

		_serviceLocator = com.dslplatform.client.Bootstrap.getLocator();
		_domainProxy = _serviceLocator.resolve(com.dslplatform.client.DomainProxy.class);
		_crudProxy = _serviceLocator.resolve(com.dslplatform.client.CrudProxy.class);
		this.ID = 0;
		this.aID = 0;
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
		final B other = (B) obj;

		return URI != null && URI.equals(other.URI);
	}

	@Override
	public String toString() {
		return URI != null ? "B(" + URI + ')' : "new B(" + super.hashCode() + ')';
	}

	private static final long serialVersionUID = 0x0097000a;

	@com.fasterxml.jackson.annotation.JsonCreator private B(
			@com.fasterxml.jackson.annotation.JacksonInject("_serviceLocator") final com.dslplatform.patterns.ServiceLocator _serviceLocator,
			@com.fasterxml.jackson.annotation.JsonProperty("URI") final String URI ,
			@com.fasterxml.jackson.annotation.JsonProperty("ID") final int ID,
			@com.fasterxml.jackson.annotation.JsonProperty("aURI") final String aURI,
			@com.fasterxml.jackson.annotation.JsonProperty("aID") final int aID) {
		this._serviceLocator = _serviceLocator;
		this._domainProxy = _serviceLocator.resolve(com.dslplatform.client.DomainProxy.class);
		this._crudProxy = _serviceLocator.resolve(com.dslplatform.client.CrudProxy.class);
		this.URI = URI;
		this.ID = ID;
		this.aURI = aURI == null ? null : aURI;
		this.aID = aID;
	}

	private int ID;

	@com.fasterxml.jackson.annotation.JsonProperty("ID")
	@com.fasterxml.jackson.annotation.JsonInclude(com.fasterxml.jackson.annotation.JsonInclude.Include.NON_EMPTY)
	public int getID()  {

		return ID;
	}

	private B setID(final int value) {

		this.ID = value;

		return this;
	}

	public static B find(final String uri) throws java.io.IOException {
		return find(uri, com.dslplatform.client.Bootstrap.getLocator());
	}
	public static B find(final String uri, final com.dslplatform.patterns.ServiceLocator locator) throws java.io.IOException {
		try {
			return (locator != null ? locator : com.dslplatform.client.Bootstrap.getLocator()).resolve(com.dslplatform.client.CrudProxy.class).read(B.class, uri).get();
		} catch (final InterruptedException e) {
			throw new java.io.IOException(e);
		} catch (final java.util.concurrent.ExecutionException e) {
			throw new java.io.IOException(e);
		}
	}
	public static java.util.List<B> find(final Iterable<String> uris) throws java.io.IOException {
		return find(uris, com.dslplatform.client.Bootstrap.getLocator());
	}
	public static java.util.List<B> find(final Iterable<String> uris, final com.dslplatform.patterns.ServiceLocator locator) throws java.io.IOException {
		try {
			return (locator != null ? locator : com.dslplatform.client.Bootstrap.getLocator()).resolve(com.dslplatform.client.DomainProxy.class).find(B.class, uris).get();
		} catch (final InterruptedException e) {
			throw new java.io.IOException(e);
		} catch (final java.util.concurrent.ExecutionException e) {
			throw new java.io.IOException(e);
		}
	}
	public static java.util.List<B> findAll() throws java.io.IOException {
		return findAll(null, null, com.dslplatform.client.Bootstrap.getLocator());
	}
	public static java.util.List<B> findAll(final com.dslplatform.patterns.ServiceLocator locator) throws java.io.IOException {
		return findAll(null, null, locator);
	}
	public static java.util.List<B> findAll(final Integer limit, final Integer offset) throws java.io.IOException {
		return findAll(limit, offset, com.dslplatform.client.Bootstrap.getLocator());
	}
	public static java.util.List<B> findAll(final Integer limit, final Integer offset, final com.dslplatform.patterns.ServiceLocator locator) throws java.io.IOException {
		try {
			return (locator != null ? locator : com.dslplatform.client.Bootstrap.getLocator()).resolve(com.dslplatform.client.DomainProxy.class).findAll(B.class, limit, offset, null).get();
		} catch (final InterruptedException e) {
			throw new java.io.IOException(e);
		} catch (final java.util.concurrent.ExecutionException e) {
			throw new java.io.IOException(e);
		}
	}
	public static java.util.List<B> search(final com.dslplatform.patterns.Specification<B> specification) throws java.io.IOException {
		return search(specification, null, null, com.dslplatform.client.Bootstrap.getLocator());
	}
	public static java.util.List<B> search(final com.dslplatform.patterns.Specification<B> specification, final com.dslplatform.patterns.ServiceLocator locator) throws java.io.IOException {
		return search(specification, null, null, locator);
	}
	public static java.util.List<B> search(final com.dslplatform.patterns.Specification<B> specification, final Integer limit, final Integer offset) throws java.io.IOException {
		return search(specification, limit, offset, com.dslplatform.client.Bootstrap.getLocator());
	}
	public static java.util.List<B> search(final com.dslplatform.patterns.Specification<B> specification, final Integer limit, final Integer offset, final com.dslplatform.patterns.ServiceLocator locator) throws java.io.IOException {
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
			return (locator != null ? locator : com.dslplatform.client.Bootstrap.getLocator()).resolve(com.dslplatform.client.DomainProxy.class).count(B.class).get().longValue();
		} catch (final InterruptedException e) {
			throw new java.io.IOException(e);
		} catch (final java.util.concurrent.ExecutionException e) {
			throw new java.io.IOException(e);
		}
	}
	public static long count(final com.dslplatform.patterns.Specification<B> specification) throws java.io.IOException {
		return count(specification, com.dslplatform.client.Bootstrap.getLocator());
	}
	public static long count(final com.dslplatform.patterns.Specification<B> specification, final com.dslplatform.patterns.ServiceLocator locator) throws java.io.IOException {
		try {
			return (locator != null ? locator : com.dslplatform.client.Bootstrap.getLocator()).resolve(com.dslplatform.client.DomainProxy.class).count(specification).get().longValue();
		} catch (final InterruptedException e) {
			throw new java.io.IOException(e);
		} catch (final java.util.concurrent.ExecutionException e) {
			throw new java.io.IOException(e);
		}
	}
	private void updateWithAnother(final namespace.myModule.B result) {
		this.URI = result.URI;

		this.a = result.a;
		this.aURI = result.aURI;
		this.aID = result.aID;
		this.ID = result.ID;
	}
	public B persist() throws java.io.IOException {

		if (this.getAURI() == null) {
			throw new IllegalArgumentException("Cannot persist instance of 'namespace.myModule.B' because reference 'a' was not assigned");
		}
		final B result;
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
	public B delete() throws java.io.IOException {
		try {
			return _crudProxy.delete(B.class, URI).get();
		} catch (final InterruptedException e) {
			throw new java.io.IOException(e);
		} catch (final java.util.concurrent.ExecutionException e) {
			throw new java.io.IOException(e);
		}
	}

	private namespace.myModule.A a;

	@com.fasterxml.jackson.annotation.JsonIgnore
	public namespace.myModule.A getA() throws java.io.IOException {

		if (a != null && !a.getURI().equals(aURI) || a == null && aURI != null)
			try {
				a = _crudProxy.read(namespace.myModule.A.class, aURI).get();
			} catch (final InterruptedException e) {
				throw new java.io.IOException(e);
			} catch (final java.util.concurrent.ExecutionException e) {
				throw new java.io.IOException(e);
			}
		return a;
	}

	public B setA(final namespace.myModule.A value) {

		if(value == null) throw new IllegalArgumentException("Property \"a\" cannot be null!");

		if(value != null && value.getURI() == null) throw new IllegalArgumentException("Reference \"myModule.A\" for property \"a\" must be persisted before it's assigned");
		this.a = value;

		this.aURI = value.getURI();

		this.aID = value.getID();
		return this;
	}

	private String aURI;

	@com.fasterxml.jackson.annotation.JsonProperty("aURI")
	public String getAURI()  {

		return this.aURI;
	}

	private int aID;

	@com.fasterxml.jackson.annotation.JsonProperty("aID")
	@com.fasterxml.jackson.annotation.JsonInclude(com.fasterxml.jackson.annotation.JsonInclude.Include.NON_EMPTY)
	public int getAID()  {

		return aID;
	}

	private B setAID(final int value) {

		this.aID = value;

		return this;
	}

	public B(
			final namespace.myModule.A a) {

		_serviceLocator = com.dslplatform.client.Bootstrap.getLocator();
		_domainProxy = _serviceLocator.resolve(com.dslplatform.client.DomainProxy.class);
		_crudProxy = _serviceLocator.resolve(com.dslplatform.client.CrudProxy.class);
		setA(a);
	}
}
