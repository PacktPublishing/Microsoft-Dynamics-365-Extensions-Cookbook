package com.packt.dynamics365;

import java.io.BufferedReader;
import java.io.IOException;
import java.io.InputStream;
import java.io.InputStreamReader;
import java.io.Reader;
import java.net.URL;
import java.nio.charset.Charset;
import java.net.HttpURLConnection;
import java.net.URLConnection;

import org.json.JSONException;
import org.json.JSONObject;
import org.json.JSONArray;


import com.microsoft.aad.adal4j.AuthenticationContext;
import com.microsoft.aad.adal4j.AuthenticationResult;

import java.util.concurrent.ExecutorService;
import java.util.concurrent.Executors;
import java.util.concurrent.Future;
import javax.naming.ServiceUnavailableException;



public class JavaDynamics365 {
	
//Azure Application Client ID
private final static String CLIENT_ID = "00000000-0000-0000-0000-000000000000";	
//CRM URL
private final static String DYNAMICS365_URL = "https://.crm6.dynamics.com";
//O365 credentials for authentication w/o login prompt
private final static String USERNAME = "ramim@.onmicrosoft.com";
private final static String PASSWORD = "";
//Azure Directory OAUTH 2.0 AUTHORIZATION ENDPOINT
private final static String AUTHORITY = "https://login.microsoftonline.com/00000000-0000-0000-0000-000000000000";
	
    public static void main(String[] args) throws IOException, JSONException, Exception {
		AuthenticationResult authenticationResult = getAccessTokenFromUserCredentials();
		// System.out.println("Access Token - " + result.getAccessToken());
        // System.out.println("Refresh Token - " + result.getRefreshToken());
        // System.out.println("ID Token - " + result.getIdToken());
		getAccounts(authenticationResult.getAccessToken(), DYNAMICS365_URL + "/api/data/v8.2/accounts?$top=2&$select=name&$orderby=name");        
    }
	
	// private static String readAll(Reader rd) throws IOException {
		// StringBuilder sb = new StringBuilder();
		// int cp;
		// while ((cp = rd.read()) != -1) {
		  // sb.append((char) cp);
		// }
		// return sb.toString();
	// }
	
private static AuthenticationResult getAccessTokenFromUserCredentials() throws Exception {
	AuthenticationContext context = null;
	AuthenticationResult result = null;
	ExecutorService service = null;
	try {
		service = Executors.newFixedThreadPool(1);
		context = new AuthenticationContext(AUTHORITY, false, service);
		
		Future<AuthenticationResult> future = context.acquireToken(DYNAMICS365_URL,
			CLIENT_ID,
			USERNAME,
			PASSWORD, null);
		result = future.get();
	} finally {
		service.shutdown();
	}
	if (result == null) {
		throw new ServiceUnavailableException("authentication result was null");
	}
	return result;
}

	public static void getAccounts(String token, String surl) throws IOException, JSONException{	
		//System.out.println(surl);
		URLConnection connection = new URL(surl).openConnection();
		connection.setRequestProperty("User-Agent", "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.11 (KHTML, like Gecko) Chrome/23.0.1271.95 Safari/537.11");
        connection.setRequestProperty("OData-MaxVersion", "4.0");
        connection.setRequestProperty("OData-Version", "4.0");
        connection.addRequestProperty("Authorization", "Bearer " + token);
        connection.setRequestProperty("Content-Type", "application/x-www-form-urlencoded");
		connection.connect();

		BufferedReader r  = new BufferedReader(new InputStreamReader(connection.getInputStream(), Charset.forName("UTF-8")));

		StringBuilder sb = new StringBuilder();
		String line;
		while ((line = r.readLine()) != null) {
			sb.append(line);
		}
		String sanitisedResult = sb.toString().substring(1,sb.toString().length() -1).trim();
		
		JSONObject json = new JSONObject(sb.toString());
		JSONArray arr = new JSONArray(json.getString("value"));
		
		for (int i=0; i <  arr.length()  ; i++) {
			JSONObject object = arr.getJSONObject(i);
			System.out.println(object.getString("name"));
		}
	}

}