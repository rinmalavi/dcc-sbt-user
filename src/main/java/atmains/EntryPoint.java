package atmains;

import java.io.ByteArrayInputStream;
import java.io.IOException;
import java.nio.charset.StandardCharsets;

import com.dslplatform.client.Bootstrap;
import namespace.myModule.A;

public class EntryPoint {
    final static String someProject =
        "username=rinmalavi@gmail.com\n" +
        "project-id=6bff118e-0ad9-4aee-813d-b292df9b9291\n" +
        "api-url=http://localhost:8999/\n" +
        "package-name=namespace";

    public static void main( String ... args) throws IOException {

        Bootstrap.init(new ByteArrayInputStream(someProject.getBytes(StandardCharsets.UTF_8)));

        new A().persist();
    }
}
