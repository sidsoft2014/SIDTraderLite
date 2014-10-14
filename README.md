SIDTraderLite
=============
<br>
SIDTraderLite is a simple, lightweight crypto-currency trading platform supporting multiple exchanges.<br>
<br>
<br>
Currently supported exchanges are:<br>
-Bittrex<br>
-BTCe<br>
-Cryptsy<br>
-Kraken<br>
-Poloniex<br>
<br>
<br>
Main features currently include:<br>
-NO API KEYS STORED!<br>
That one I thought deserved capitalistion. Rather than going for the approach many other platforms take of storing API keys using encryption and then decrypting for use, this application only stores a HMAC512 hash of your key using enchaned SHA256 encryption. This means your actual private keys are impossible for a hacker to retrieve.<br>
<br>
-Password protected user accounts.<br>
As with API keys, passwords are not stored. Instead the application takes the proper approach of storing an irreversible salted hash of your password.<br>
<br>
-Dynamically generated, periodically checked market lists.<br>
If a new market is added while the application is running it will alert you so you can take early action.<br>
<br>
-Dynamically calculated 'rates' panel<br>
Upon loading an exchange the application identifies currency bases and displays the last trade for base pairs (eg LTC/BTC, BTC/USD)<br>
<br>
-Zoomable charts for every market.<br>
Whether the exchange has chart data available through the API or not, this application will present you with OHLC charts for every market. As well as being zoomable they are also annotated with highest price, lowest price and time period.<br>
<br>
-Conditional Orders<br>
Basically internally stored stop-limit orders. Set the conditions and the application will perform the trade if conditions are met.<br>
<br>
-Market Settings<br>
Intended mainly to display min\max price\volume when dictated by the exchange, these settings can be altered by the user if desired (for such purposes as preventing mis-reading a quickly entered price and selling for 0.001 rather than 0.01).<br>
<br>
-Adjustable update periods<br>
Each exchange can be updated from it's default update period of a few minutes to any timespan you would like.<br>
<br>
-Big Red 'Wipe' Button<br>
Last on the list, for added security there is a big red button that will wipe all stored user data instantly.<br>
<br><br>
Planned future features:<br>
- MintPal API integration<br>
- Vircurex API integration<br>
- Timed orders\Time limited conditional orders <br>
- User history<br>
<br><br>
Screenshots of the application can be found here - http://imgur.com/a/F2cnx<br>
A pre-compiled download of the application can be found here - https://mega.co.nz/#!a4EWhSCJ!KoJnPGaEpNmN_oo2VPD8KFMikLi5M0SwmAfgiAGN5ZY<br>
<br><br>
Finally.....<br>
The library's and classes in this project may be freely used in your own non-commercial projects without limitation. If you would like to use any parts of this project in a new commercial product please contact me via the email included in the user guide first.
