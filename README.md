SIDTraderLite
=============

SIDTraderLite is a simple, lightweight crypto-currency trading platform supporting multiple exchanges.


Currently supported exchanges are:
-BTCe
-Cryptsy
-Kraken
-Poloniex


Main features currently include:
-NO API KEYS STORED!
That one I thought deserved capitalistion. Rather than going for the approach many other platforms take of storing API keys using encryption and then decrypting for use, this application only stores a HMAC512 hash of your key using enchaned SHA256 encryption. This means your actual private keys are impossible for a hacker to retrieve.

-Password protected user accounts.
As with API keys, passwords are not stored. Instead the application takes the proper approach of storing an irreversible salted hash of your password.

-Dynamically generated, periodically checked market lists.
If a new market is added while the application is running it will alert you so you can take early action.

-Dynamically calculated 'rates' panel
Upon loading an exchange the application identifies currency bases and displays the last trade for base pairs (eg LTC/BTC, BTC/USD)

-Zoomable charts for every market.
Whether the exchange has chart data available through the API or not, this application will present you with OHLC charts for every market. As well as being zoomable they are also annotated with highest price, lowest price and time period.

-Conditional Orders
Basically internally stored stop-limit orders. Set the conditions and the application will perform the trade if conditions are met.

-Market Settings
Intended mainly to display min\max price\volume when dictated by the exchange, these settings can be altered by the user if desired (for such purposes as preventing mis-reading a quickly entered price and selling for 0.001 rather than 0.01).

-Adjustable update periods
Each exchange can be updated from it's default update period of a few minutes to any timespan you would like.

-Big Red 'Wipe' Button
Last on the list, for added security there is a big red button that will wipe all stored user data instantly.

Planned future features:
- MintPal API integration
- Vircurex API integration
- Timed orders\Time limited conditional orders 
- User history

Screenshots of the application can be found here - http://imgur.com/a/F2cnx
A pre-compiled download of the application can be found here - https://mega.co.nz/#!i9dlWY4S!Eju60GvV68GTaRKHeAcJrDLeRbcqaeJjO4B6ZT85gTc

Finally.....
The library's and classes in this project may be freely used in your own non-commercial projects without limitation. If you would like to use any parts of this project in a new commercial product please contact me via the email included in the user guide first.
