using System;

namespace GildedRose
{
    public class CatalogItem
    {
        private const int DailyChange = 1;
        private const int SulfurasDefault = 80;
        private const int ItemCap = 50;

        public int ID { get; set; }

        public string Name { get; set; }

        public string Category { get; set; }

        public int InitialSellin { get; set; }

        public int Sellin { get; set; }

        public int InitialQuality { get; set; }
        
        public int Quality { get; set; }

        public DateTime DateUploaded { get; set; }

        public void EvaluateQuality(){
            //TODO verify the daily decrease is 1
            if(this.Category == "Sulfuras"){
                this.Quality = SulfurasDefault;
            }
            else if(this.Category == "Backstage passes"){
                var concertDay = DateUploaded.AddDays(this.InitialSellin);
                System.TimeSpan dateDifference = concertDay.Subtract(DateTime.Now);
                var daysToConcert = dateDifference.Days;
                System.TimeSpan daysPassed = DateTime.Now.Subtract(DateUploaded);
                var daysPassedSinceUpload = daysPassed.Days;

                if(daysToConcert < 0){
                    this.Quality = 0;
                }
                else{
                    this.Quality = InitialQuality + (DailyChange * daysPassedSinceUpload);
                    if(daysToConcert < 10){
                        var premiumDays = 10 - daysToConcert;
                        this.Quality += daysPassedSinceUpload < premiumDays ? DailyChange * daysPassedSinceUpload: DailyChange * premiumDays;
                    }
                    if(daysToConcert < 5){
                        var premiumDays = 5 - daysToConcert;
                        this.Quality += daysPassedSinceUpload < premiumDays ? DailyChange * daysPassedSinceUpload: DailyChange * premiumDays;
                    }
                }
                
                this.Quality = this.Quality > ItemCap ? ItemCap : this.Quality;
            }
            else if(this.Category == "Conjured"){
                System.TimeSpan dateDifference = DateTime.Now.Subtract(this.DateUploaded);
                var newQuality = InitialQuality - (2 * DailyChange * dateDifference.Days);
                this.Quality = newQuality < 0 ? 0 : newQuality; 
            }
            else{
                if(this.Name == "Aged Brie"){
                    System.TimeSpan dateDifference = DateTime.Now.Subtract(this.DateUploaded);
                    var newQuality = InitialQuality + (DailyChange * dateDifference.Days);
                    this.Quality = newQuality > ItemCap ? ItemCap : newQuality;
                }
                else{
                    System.TimeSpan dateDifference = DateTime.Now.Subtract(this.DateUploaded);
                    var newQuality = InitialQuality - (DailyChange * dateDifference.Days);
                    this.Quality = newQuality < 0 ? 0 : newQuality; 
                }
            }
        }

        public void EvaluateSellin(){
            if(this.Category == "Sulfuras"){
                this.Sellin = this.InitialSellin;
            }
            else{
                System.TimeSpan dateDifference = DateTime.Now.Subtract(this.DateUploaded);
                this.Sellin = this.InitialSellin - dateDifference.Days;
            }
        }
    }
}
