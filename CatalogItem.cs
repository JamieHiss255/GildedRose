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

        public void EvaluateQuality(DateTime dayForQualityEval){
            System.TimeSpan dateSpanSinceUpload = dayForQualityEval.Subtract(this.DateUploaded);
            var daysSinceUpload = dateSpanSinceUpload.Days;
            var sellByDate = DateUploaded.AddDays(this.InitialSellin);
            System.TimeSpan dateSpanUntilSellBy = sellByDate.Subtract(dayForQualityEval);
            var daysUntilSellBy = dateSpanUntilSellBy.Days;

            //TODO verify the daily decrease is 1
            if(this.Category.ToLower() == "sulfuras"){
                this.Quality = SulfurasDefault;
            }
            else if(this.Category.ToLower() == "backstage passes"){

                if(daysUntilSellBy < 0){
                    this.Quality = 0;
                }
                else{
                    this.Quality = InitialQuality + (DailyChange * daysSinceUpload);
                    if(daysUntilSellBy < 10){
                        var premiumDays = 10 - daysUntilSellBy;
                        this.Quality += daysSinceUpload < premiumDays ? DailyChange * daysSinceUpload: DailyChange * premiumDays;
                    }
                    if(daysUntilSellBy < 5){
                        var premiumDays = 5 - daysUntilSellBy;
                        this.Quality += daysSinceUpload < premiumDays ? DailyChange * daysSinceUpload: DailyChange * premiumDays;
                    }
                }
                
                this.Quality = this.Quality > ItemCap ? ItemCap : this.Quality;
            }
            else if(this.Category.ToLower() == "conjured"){
                var newQuality = InitialQuality - (2 * DailyChange * daysSinceUpload);
                this.Quality = newQuality < 0 ? 0 : newQuality; 
            }
            else{
                if(this.Name.ToLower() == "aged brie"){
                    var newQuality = InitialQuality + (DailyChange * daysSinceUpload);
                    this.Quality = newQuality > ItemCap ? ItemCap : newQuality;
                }
                else if(daysUntilSellBy < 0){
                    var newQuality = InitialQuality - (2 * DailyChange * daysSinceUpload);
                    this.Quality = newQuality < 0 ? 0 : newQuality; 
                }
                else{
                    var newQuality = InitialQuality - (DailyChange * daysSinceUpload);
                    this.Quality = newQuality < 0 ? 0 : newQuality; 
                }
            }
        }

        public void EvaluateSellin(DateTime dayForQualityEval){
            if(this.Category.ToLower() == "sulfuras"){
                this.Sellin = this.InitialSellin;
            }
            else{
                System.TimeSpan dateDifference = dayForQualityEval.Subtract(this.DateUploaded);
                var newSellin = this.InitialSellin - dateDifference.Days;
                this.Sellin = newSellin < 0 ? 0: newSellin;
            }
        }
    }
}
