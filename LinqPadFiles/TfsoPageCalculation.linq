<Query Kind="Statements">
  <Namespace>System.Globalization</Namespace>
</Query>

var callCost = 0.017;
var minimumCallsForPosting = 6;
var callsPrPage = 4;
var pagesCount = 1;

var totalCost = (minimumCallsForPosting + (pagesCount * callsPrPage)) * callCost;
(totalCost / pagesCount).Dump();