// The file contents for the current environment will overwrite these during build.
// The build system defaults to the dev environment which uses `environment.ts`, but if you do
// `ng build --env=prod` then `environment.prod.ts` will be used instead.
// The list of which env maps to which file can be found in `angular-cli.json`.
//import { AppConfiguration } from "read-appsettings-json";

export const environment = {
  production: false,
  hmr: false,
  appConfig: 'appconfig.json',

  // AppConfiguration.Setting().Application.ServiceUrl

  // Testing
  //apiUrl: 'http://51.79.209.55:6770'

  // Live
  apiUrl: 'http://51.79.209.55:6767'

  // LocalHost
   //apiUrl:'https://localhost:44324',

};
