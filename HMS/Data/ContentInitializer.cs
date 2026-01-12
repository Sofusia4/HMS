using HMS.Models;
using static System.Net.Mime.MediaTypeNames;

namespace HMS.Data
{
	public class ContentInitializer
	{
		public static async Task HotelsInitializeAsync(ApplicationContext context)
		{
			if (!context.Hotels.Any())
			{
				context.Hotels.AddRange
					(
						new Hotel 
						{ 
							Name = "Прем'єр Палац Готель", 
							NameEng = "Premier Palace Hotel",
							Description = "Premier Palace Hotel - київський за сутністю та атмосферою, розкішний історичний готель, перлина в колекції Premier Hotels and Resorts. Його історія унікальна і тісно пов'язана з історією Києва. Це одна з визначних пам'яток столиці, відвідуванню якої варто присвятити окремий час.",
							DescriptionEng = "Premier Palace Hotel – Kyiv-featured both in essence and atmosphere – is a luxurious historic hotel, the crown jewel in Premier Hotels and Resorts collection. Its history is unique and closely connected with the history of Kyiv. This is one of the capital attractions; it is worth taking some time to visit it.",
							City = "Київ",
							CityEng = "Kyiv",
							Address = "бул. Т. Шевченка / вул. Є. Чикаленка, 5-7/29, Київ 01004, Україна",
							AddressEng = "5-7/29 T. Shevchenka Blvd. / Y. Chykalenka St., Kyiv 01004, Ukraine"
						},
						new Hotel
						{
							Name = "Прем'єр Готель Либідь",
							NameEng = "Premier Hotel Lybid",
							Description = "Premier Hotel Lybid знаходиться в діловому та історичному центрі столиці України. Із вікон готелю відкриваються чудові види на площу Перемоги і бульвар Шевченка. Поруч з готелем знаходиться торговий центр «Україна», Національний Цирк, зручна розв’язка наземного міського транспорту.",
							DescriptionEng = "Premier Hotel Lybid is located in the business and historical center of the capital of Ukraine. Hotel offers excellent views of Peremohy Square and Shevchenko Boulevard. Ukraina Shopping Mall and National Circus are located near the hotel; aboveground transport junction is convenient.",
							City = "Київ",
							CityEng = "Kyiv",
							Address = "площа Перемоги, 1 м. Київ 01135, Україна",
							AddressEng = "1 Peremohy Square, Kyiv 01135, Ukraine"
						},
						new Hotel
						{
							Name = "Прем'єр Готель Дністер",
							NameEng = "Premier Hotel Dnister",
							Description = "Premier Hotel Dnister – найгостинніший у Львові з неймовірною панорамою Старого міста, що відкривається з його вікон. Вдало розташувавшись на пагорбі навпроти мальовничого парку, готель знаходиться всього за декілька кроків від Старого Міста, яке внесено до Списку Всесвітньої спадщини ЮНЕСКО, а також від найвизначніших архітектурних пам’яток Львова. ",
							DescriptionEng = "Premier Hotel Dnister is the most hospitable hotel in Lviv offering a captivating panorama of the Old Town. Scrambled upon a hill opposite the picturesque park, the Hotel is within walking distance to the Old Town, a UNESCO World Heritage Site, and the main attractions of the city.",
							City = "Львів",
							CityEng = "Lviv",
							Address = "вул. Матейка, 6, Львів 79007, Україна",
							AddressEng = "6 Mateika St, Lviv 79007, Ukraine"
						}
					);
				context.SaveChanges();
			}
		}
		public static async Task RoomsInitializeAsync(ApplicationContext context)
		{			
			if (!context.Rooms.Any())
			{
				context.Rooms.AddRange
					(
						new Room
						{
							Number = 100,
							RoomType = RoomType.Standard,
							PricePerNight = 7000,
							Capacity = 2,
							Description = "Однокімнатний номер середньою площею 26 м2, ванна – 4,8 м2 (більшість ванних обладнані біде). Ширина ліжка 160 см. Номер чудово підходить для тих, хто подорожує один або приїхав у Київ у службових справах.",
							FullImageName = "/roomPhotos/1/1.jpg",
							Image = "1.jpg",
							HotelId = context.Hotels.FirstOrDefault(e => e.NameEng.Equals("Premier Palace Hotel")).Id.ToString(),
							Hotel = context.Hotels.FirstOrDefault(e => e.NameEng.Equals("Premier Palace Hotel"))
						},
						new Room
						{
							Number = 200,
							RoomType = RoomType.Luxury,
							PricePerNight = 15000,
							Capacity = 3,
							Description = "Двокімнатний номер середньою площею 37 м2. Номер складається з вітальні та опочивальні. В номері – гостьовий туалет та ванна кімната (обладнана біде). Ширина ліжка від 160 см.",
							FullImageName = "/roomPhotos/2/1.jpg",
							Image = "1.jpg",
							HotelId = context.Hotels.FirstOrDefault(e => e.NameEng.Equals("Premier Palace Hotel")).Id.ToString(),
							Hotel = context.Hotels.FirstOrDefault(e => e.NameEng.Equals("Premier Palace Hotel"))
						},
						new Room
						{
							Number = 300,
							RoomType = RoomType.Suite,
							PricePerNight = 16000,
							Capacity = 4,
							Description = "Двокімнатний номер середньою площею 37 м2. Номер складається з вітальні та опочивальні. В номері – гостьовий туалет та ванна кімната (обладнана біде). Ширина ліжка від 160 см.",
							FullImageName = "/roomPhotos/3/1.jpg",
							Image = "1.jpg",
							HotelId = context.Hotels.FirstOrDefault(e => e.NameEng.Equals("Premier Palace Hotel")).Id.ToString(),
							Hotel = context.Hotels.FirstOrDefault(e => e.NameEng.Equals("Premier Palace Hotel"))
						},
						new Room
						{
							Number = 101,
							RoomType = RoomType.Standard,
							PricePerNight = 10000,
							Capacity = 2,
							Description = "Однокімнатний двомісний номер середньою площею 30 м2, ванна – 5,1 м2 (ванна кімната обладнана біде). Два окремі ліжка завширшки від 100 до 120 см кожне. Можливе розміщення в номері з одним двома роздільними.",
							FullImageName = "/roomPhotos/4/1.jpg",
							Image = "1.jpg",
							HotelId = context.Hotels.FirstOrDefault(e => e.NameEng.Equals("Premier Palace Hotel")).Id.ToString(),
							Hotel = context.Hotels.FirstOrDefault(e => e.NameEng.Equals("Premier Palace Hotel"))
						},
						new Room
						{
							Number = 301,
							RoomType = RoomType.Suite,
							PricePerNight = 21000,
							Capacity = 6,
							Description = "Двокімнатний номер середньою площею 54 м2. Номер складається з вітальні та опочивальні. В номері – гостьовий туалет та ванна кімната (обладнана біде). Ширина ліжка від 160 до 180 см.",
							FullImageName = "/roomPhotos/5/1.jpg",
							Image = "1.jpg",
							HotelId = context.Hotels.FirstOrDefault(e => e.NameEng.Equals("Premier Palace Hotel")).Id.ToString(),
							Hotel = context.Hotels.FirstOrDefault(e => e.NameEng.Equals("Premier Palace Hotel"))
						},
						new Room
						{
							Number = 100,
							RoomType = RoomType.Standard,
							PricePerNight = 10000,
							Capacity = 2,
							Description = "Однокімнатний двомісний номер середньою площею 30,3 м2, ванна – 5,3 м2 (більшість ванних обладнані біде). Ширина ліжка 180 см (King Size). Номер ідеально підходить для пар та ділових людей, які потребують більше комфорту. Можливе розміщення в номері з одним широким ліжком.",
							FullImageName = "/roomPhotos/6/1.jpg",
							Image = "1.jpg",
							HotelId = context.Hotels.FirstOrDefault(e => e.NameEng.Equals("Premier Hotel Lybid")).Id.ToString(),
							Hotel = context.Hotels.FirstOrDefault(e => e.NameEng.Equals("Premier Hotel Lybid"))
						},
						new Room
						{
							Number = 101,
							RoomType = RoomType.Standard,
							PricePerNight = 11000,
							Capacity = 2,
							Description = "Однокімнатний номер середньою площею 26 м2, ванна – 4,8 м2 (більшість ванних обладнані біде). Ширина ліжка 160 см. Номер чудово підходить для тих, хто подорожує один або приїхав у Київ у службових справах.",
							FullImageName = "/roomPhotos/7/1.jpg",
							Image = "1.jpg",
							HotelId = context.Hotels.FirstOrDefault(e => e.NameEng.Equals("Premier Hotel Lybid")).Id.ToString(),
							Hotel = context.Hotels.FirstOrDefault(e => e.NameEng.Equals("Premier Hotel Lybid"))
						},
						new Room
						{
							Number = 200,
							RoomType = RoomType.Luxury,
							PricePerNight = 17000,
							Capacity = 3,
							Description = "Двокімнатний номер середньою площею 37 м2. Номер складається з вітальні та опочивальні. В номері – гостьовий туалет та ванна кімната (обладнана біде). Ширина ліжка від 160 см.",
							FullImageName = "/roomPhotos/8/1.jpg",
							Image = "1.jpg",
							HotelId = context.Hotels.FirstOrDefault(e => e.NameEng.Equals("Premier Hotel Lybid")).Id.ToString(),
							Hotel = context.Hotels.FirstOrDefault(e => e.NameEng.Equals("Premier Hotel Lybid"))
						},
						new Room
						{
							Number = 300,
							RoomType = RoomType.Suite,
							PricePerNight = 19000,
							Capacity = 5,
							Description = "Двокімнатний номер середньою площею 54 м2. Номер складається з вітальні та опочивальні. В номері – гостьовий туалет та ванна кімната (обладнана біде). Ширина ліжка від 160 до 180 см.",
							FullImageName = "/roomPhotos/9/1.jpg",
							Image = "1.jpg",
							HotelId = context.Hotels.FirstOrDefault(e => e.NameEng.Equals("Premier Hotel Lybid")).Id.ToString(),
							Hotel = context.Hotels.FirstOrDefault(e => e.NameEng.Equals("Premier Hotel Lybid"))
						},
						new Room
						{
							Number = 100,
							RoomType = RoomType.Standard,
							PricePerNight = 7000,
							Capacity = 2,
							Description = "Однокімнатний номер середньою площею 26 м2, ванна – 4,8 м2 (більшість ванних обладнані біде). Ширина ліжка 160 см. Номер чудово підходить для тих, хто подорожує один або приїхав у Київ у службових справах.",
							FullImageName = "/roomPhotos/10/1.jpg",
							Image = "1.jpg",
							HotelId = context.Hotels.FirstOrDefault(e => e.NameEng.Equals("Premier Hotel Dnister")).Id.ToString(),
							Hotel = context.Hotels.FirstOrDefault(e => e.NameEng.Equals("Premier Hotel Dnister"))
						},
						new Room
						{
							Number = 101,
							RoomType = RoomType.Standard,
							PricePerNight = 9000,
							Capacity = 2,
							Description = "Однокімнатний двомісний номер середньою площею 30,3 м2, ванна – 5,3 м2 (більшість ванних обладнані біде). Ширина ліжка 180 см (King Size). Номер ідеально підходить для пар та ділових людей, які потребують більше комфорту. Можливе розміщення в номері з одним широким ліжком.",
							FullImageName = "/roomPhotos/11/1.jpg",
							Image = "1.jpg",
							HotelId = context.Hotels.FirstOrDefault(e => e.NameEng.Equals("Premier Hotel Dnister")).Id.ToString(),
							Hotel = context.Hotels.FirstOrDefault(e => e.NameEng.Equals("Premier Hotel Dnister"))
						},
						new Room
						{
							Number = 200,
							RoomType = RoomType.Luxury,
							PricePerNight = 12000,
							Capacity = 3,
							Description = "Двокімнатний номер середньою площею 54 м2. Номер складається з вітальні та опочивальні. В номері – гостьовий туалет та ванна кімната (обладнана біде). Ширина ліжка від 160 до 180 см.",
							FullImageName = "/roomPhotos/12/1.jpg",
							Image = "1.jpg",
							HotelId = context.Hotels.FirstOrDefault(e => e.NameEng.Equals("Premier Hotel Dnister")).Id.ToString(),
							Hotel = context.Hotels.FirstOrDefault(e => e.NameEng.Equals("Premier Hotel Dnister"))
						},
						new Room
						{
							Number = 201,
							RoomType = RoomType.Luxury,
							PricePerNight = 14000,
							Capacity = 4,
							Description = "Двокімнатний номер середньою площею 37 м2. Номер складається з вітальні та опочивальні. В номері – гостьовий туалет та ванна кімната (обладнана біде). Ширина ліжка від 160 см.",
							FullImageName = "/roomPhotos/13/1.jpg",
							Image = "1.jpg",
							HotelId = context.Hotels.FirstOrDefault(e => e.NameEng.Equals("Premier Hotel Dnister")).Id.ToString(),
							Hotel = context.Hotels.FirstOrDefault(e => e.NameEng.Equals("Premier Hotel Dnister"))
						},
						new Room
						{
							Number = 300,
							RoomType = RoomType.Suite,
							PricePerNight = 18000,
							Capacity = 6,
							Description = "Двокімнатний номер середньою площею 54 м2. Номер складається з вітальні та опочивальні. В номері – гостьовий туалет та ванна кімната (обладнана біде). Ширина ліжка від 160 до 180 см.",
							FullImageName = "/roomPhotos/14/1.jpg",
							Image = "1.jpg",
							HotelId = context.Hotels.FirstOrDefault(e => e.NameEng.Equals("Premier Hotel Dnister")).Id.ToString(),
							Hotel = context.Hotels.FirstOrDefault(e => e.NameEng.Equals("Premier Hotel Dnister"))
						}
					);
				context.SaveChanges();
			}
		}
	}

}
