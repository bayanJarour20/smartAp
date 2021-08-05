export default {
  // Endpoints
  loginEndpoint: 'Account/Login',
  registerEndpoint: '/jwt/register',
  refreshEndpoint: 'Account/RefrshToken',
  logoutEndpoint: '/jwt/logout',

  // This will be prefixed in authorization header with token
  // e.g. Authorization: Bearer <token>
  tokenType: 'Bearer',

  // Value of this property will be used as key to store JWT token in storage
  storageTokenKeyName: 'SmartStartAccessToken',
  storageRefreshTokenKeyName: 'SmartStartRefreshToken',
}
