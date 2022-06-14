using Google.Apis.Auth.OAuth2;
using Google.Apis.Services;
using Google.Apis.Sheets.v4;
using UnityEngine;

public static class SheetAPI {
	public static SheetsService Service;

	public static void InitService(string ServiceAccountID, string PrivateKey) {
		if (Service != null) return;
		var initializer = new ServiceAccountCredential.Initializer(ServiceAccountID);
		var credential = new ServiceAccountCredential(initializer.FromPrivateKey(PrivateKey));

		Service = new SheetsService(
			new BaseClientService.Initializer {
				HttpClientInitializer = credential,
			}
		);
	}


	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
	private static void ResetService() {
		Service = null;
	}
}
