using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace NZH.Common.Extensions
{
    /// <summary>
    /// stirng扩展
    /// </summary>
    public static class ExtendString
    {
        /// <summary>
        /// 转全角(SBC case)
        /// </summary>
        /// <param name="input">任意字符串</param>
        /// <returns>全角字符串</returns>
        public static string ToSBC(this string input)
        {
            char[] c = input.ToCharArray();
            for (int i = 0; i < c.Length; i++)
            {
                if (c[i] == 32)
                {
                    c[i] = (char)12288;
                    continue;
                }
                if (c[i] < 127)
                {
                    c[i] = (char)(c[i] + 65248);
                }
            }
            return new string(c);
        }

        /// <summary>
        /// 转半角(DBC case)
        /// </summary>
        /// <param name="input">任意字符串</param>
        /// <returns>半角字符串</returns>
        public static string ToDBC(this string input)
        {
            char[] c = input.ToCharArray();
            for (int i = 0; i < c.Length; i++)
            {
                if (c[i] == 12288)
                {
                    c[i] = (char)32;
                    continue;
                }
                if (c[i] > 65280 && c[i] < 65375)
                {
                    c[i] = (char)(c[i] - 65248);
                }
            }
            return new string(c);
        }

        internal const int LOCALE_SYSTEM_DEFAULT = 0x0800;
        internal const int LCMAP_SIMPLIFIED_CHINESE = 0x02000000;
        internal const int LCMAP_TRADITIONAL_CHINESE = 0x04000000;

        /// <summary> 
        /// 使用系统的kernel.dll做简繁体转换,逐字转换
        /// <para></para> 
        /// </summary> 
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        internal static extern int LCMapString(int Locale, int dwMapFlags, string lpSrcStr, int cchSrc, [Out] string lpDestStr, int cchDest);

        /// <summary>
        /// 转简体
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToChineseGB2312ForAPI(this string value)
        {
            String str = new String(' ', value.Length);
            int tReturn = LCMapString(LOCALE_SYSTEM_DEFAULT, LCMAP_SIMPLIFIED_CHINESE, value, value.Length, str, value.Length);
            return str;
        }

        /// <summary>
        ///  转繁体
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToChineseBIG5ForAPI(this string value)
        {
            String str = new String(' ', value.Length);
            int tReturn = LCMapString(LOCALE_SYSTEM_DEFAULT, LCMAP_TRADITIONAL_CHINESE, value, value.Length, str, value.Length);
            return str;
        }

        /// <summary>
        /// 简体字库
        /// </summary>
        private static string SimplifiedChinese = @"皑蔼碍爱隘翱袄奥懊澳捌靶把耙坝霸罢摆败稗扳颁办绊帮梆绑镑谤褒剥薄雹堡饱宝报鲍辈贝钡狈备惫绷笔碧蓖蔽毕毙闭敝弊壁臂避陛鞭边编贬变辨辩辫鳖憋别瘪濒滨宾摈饼拨钵勃铂箔驳卜补埠簿部怖擦参蚕残惭惨灿苍舱仓沧厕侧册测层诧搀掺蝉馋谗缠铲产阐颤昌猖场尝长偿肠厂敞畅钞车撤掣彻澈郴尘陈衬撑称惩澄诚骋痴迟驰耻齿炽冲虫崇宠抽酬畴踌筹绸瞅丑橱厨躇锄雏础储矗搐触处揣传疮闯创锤纯蠢戳绰疵茨磁雌辞慈瓷词赐聪葱囱从丛凑粗醋簇促蹿篡窜摧瘁粹淬磋撮搓措挫错搭达傣戴带贷担单郸掸胆氮但惮淡诞弹蛋当挡党荡档捣蹈岛祷导盗蹬灯邓滴迪敌涤翟递缔颠掂滇碘点靛垫电惦奠淀殿钓调跌爹碟蝶迭谍叠钉顶锭订东董懂动栋冻斗犊独读堵睹赌镀锻断缎堆兑队对墩吨蹲敦顿囤钝盾遁掇哆夺垛鹅额讹娥恶扼遏鄂饿恩儿尔饵洱贰发罚阀珐藩矾钒烦范贩饭访纺飞废沸费纷坟奋愤粪丰枫蜂峰锋风疯烽逢冯缝讽凤肤孵辐抚辅赋复傅腹负富讣妇缚咐噶嘎该改概钙盖溉干赶感秆敢赣冈刚钢纲岗篙皋糕搞镐搁鸽胳疙阁隔铬个给耕羹埂耿梗龚宫巩贡钩沟构购够箍蛊顾剐关观管馆惯贯广瑰规硅归龟闺轨诡柜跪贵刽辊滚棍锅郭国裹过哈骸骇韩汉阂涸赫褐鹤贺嘿横轰鸿红后壶葫护沪户哗华画划话槐徊怀淮坏欢环桓还缓换唤痪豢焕涣黄磺蝗簧谎挥辉徽恢蛔毁悔慧惠晦贿秽会烩汇讳诲绘荤浑豁伙获霍货祸击机畸稽积箕饥激讥鸡姬绩缉极棘辑级挤几脊蓟冀剂悸济计记既际继纪枷夹荚颊贾钾假稼价驾嫁歼监坚笺间艰缄茧检柬碱硷拣捡简俭剪减荐槛鉴践贱见键舰剑饯渐溅涧浆疆蒋桨奖讲酱蕉椒礁焦胶浇骄娇搅铰矫侥脚饺缴绞剿教酵轿较窖揭秸阶截节茎惊粳经警景颈静境敬镜径痉靖竟竞净纠厩救旧驹举据锯惧剧捐鹃绢撅攫杰捷睫竭洁结戒藉芥诫届紧锦仅谨进靳晋烬浸尽劲荆兢觉决诀绝钧军峻俊竣浚郡骏开揩凯慨堪勘坎砍康慷糠磕颗壳咳课垦恳抠库裤夸块侩宽矿旷况亏岿窥葵奎魁傀馈愧溃坤扩廓阔喇蜡腊莱来赖蓝婪栏拦篮阑兰澜谰揽览懒缆烂滥廊捞劳烙涝勒乐镭垒擂肋类泪楞冷厘梨犁黎篱狸离漓理里鲤礼莉荔丽厉励砾历傈痢粒沥隶璃哩俩联莲连镰廉怜涟帘敛脸链恋炼练粮凉两辆量谅撩聊僚疗燎寥辽潦撂镣猎霖临邻鳞淋凛赁拎菱零龄铃凌灵陵岭领馏刘龙聋咙笼窿隆垄拢陇楼娄搂篓漏陋芦卢颅庐炉掳卤虏鲁赂禄录陆戮驴吕铝侣旅履屡缕虑率滤绿峦挛孪滦乱抡轮伦仑沦纶论萝螺罗逻锣箩骡裸落洛骆络妈玛码蚂马骂嘛吗埋买麦卖迈脉瞒馒蛮满蔓谩猫锚铆贸么霉没媒镁门闷们萌蒙檬盟锰猛梦谜弥秘觅绵冕勉娩缅瞄藐渺庙蔑灭悯闽螟鸣铭谬摸摹蘑谋亩姆钠纳难囊挠脑恼闹淖呢馁腻溺蔫撵捻娘酿鸟捏聂孽啮镊镍涅柠狞凝宁拧泞钮纽脓浓农疟挪懦糯诺哦欧鸥殴藕呕偶沤攀潘盘磐盼畔庞中国昆山博爱天下耪赔喷抨鹏骗飘频贫聘苹萍凭瓶评屏坡泼颇扑铺朴谱脐齐骑岂启契砌气弃讫掐牵扦钎铅迁签谦乾黔钱钳潜遣浅谴堑嵌歉枪呛腔羌墙蔷强抢橇锹桥乔侨鞘撬翘峭窍窃钦亲轻氢倾卿顷请庆琼穷趋区躯驱渠取娶龋趣颧权醛痊劝缺炔瘸却鹊让饶扰绕惹热韧认纫荣绒揉褥软锐闰润弱撒洒萨腮鳃塞赛伞桑嗓丧搔骚扫涩杀纱傻啥煞筛晒闪陕擅赡缮墒伤赏梢捎稍烧绍奢赊蛇赦摄慑涉设砷绅审婶肾渗声绳胜圣师狮湿诗尸时蚀实识驶势释饰视试寿瘦兽蔬枢输书赎孰熟薯暑曙署蜀黍鼠属术树竖数漱帅双谁税吮瞬顺舜说硕烁丝嗣饲耸怂颂讼诵搜艘擞嗽苏诉肃酸蒜虽绥髓碎岁孙损笋蓑梭唆缩琐索锁獭挞蹋抬泰酞摊贪瘫滩坛檀痰潭谭谈毯袒碳探叹汤糖烫涛滔绦腾疼誊锑题蹄啼体替嚏惕涕剃屉条眺贴铁帖厅听烃铜统头图涂团颓腿蜕褪退臀拖脱鸵驮驼椭洼袜豌弯湾顽万网韦违桅围唯惟为潍维苇萎伟伪纬谓慰卫温闻纹吻稳紊问嗡翁瓮挝蜗涡窝斡握呜钨乌诬无芜吴坞雾务误锡牺稀膝犀檄袭习媳铣戏细虾辖峡侠狭厦锨鲜纤咸贤衔舷闲显险现献县腺馅羡宪陷限线厢镶乡详响项萧霄销晓啸楔些歇蝎鞋协挟携胁谐写械卸蟹懈泄泻谢锌衅兴汹锈袖绣墟戌需虚嘘须徐许蓄绪续轩悬选癣眩绚靴薛学勋询寻驯训讯逊压鸦鸭哑亚讶焉咽阉烟淹盐严颜阎艳厌砚雁唁彦谚验鸯杨扬佯疡阳痒养样漾邀腰瑶摇尧遥窑谣姚药椰噎爷页掖业叶腋夜液壹医揖铱颐夷遗仪疑彝蚁艺亿臆逸肄疫裔毅忆义诣议谊译异翼翌绎荫殷阴银饮樱婴鹰应缨莹萤营荧蝇颖硬哟拥佣臃痈庸雍踊蛹咏涌优忧邮铀犹游釉诱淤盂虞愚舆逾鱼愉渝渔隅娱与屿禹语吁峪御狱誉预豫驭鸳渊辕园员圆猿源缘远苑愿怨院约越跃钥岳粤悦阅云郧匀陨运蕴酝晕韵砸杂灾载攒暂赞赃脏葬遭糟凿藻枣灶燥责择则泽贼赠扎札轧铡闸诈斋债寨瞻毡盏斩辗崭展蘸栈战站湛绽张涨帐账胀赵蛰辙锗蔗这斟甄砧臻贞针侦诊震振镇阵蒸挣睁狰帧郑证织职植殖执纸挚掷帜质钟终种肿众洲诌粥轴皱宙昼骤猪诸诛烛煮瞩嘱贮铸筑驻拽专砖转撰赚篆桩庄装妆撞壮状锥赘坠缀谆著浊兹资滓渍鬃棕踪宗综总纵邹揍诅组钻纂致钟么为只凶准启板里雳余链泄";

        /// <summary>
        /// 繁体字库
        /// </summary>
        private static string TraditionalChinese = @"皚藹礙愛隘翺襖奧懊澳捌靶把耙壩霸罷擺敗稗扳頒辦絆幫梆綁鎊謗褒剝薄雹堡飽寶報鮑輩貝鋇狽備憊繃筆碧蓖蔽畢斃閉敝弊壁臂避陛鞭邊編貶變辨辯辮鼈憋別癟瀕濱賓擯餅撥缽勃鉑箔駁蔔補埠簿部怖擦參蠶殘慚慘燦蒼艙倉滄廁側冊測層詫攙摻蟬饞讒纏鏟産闡顫昌猖場嘗長償腸廠敞暢鈔車撤掣徹澈郴塵陳襯撐稱懲澄誠騁癡遲馳恥齒熾沖蟲崇寵抽酬疇躊籌綢瞅醜櫥廚躇鋤雛礎儲矗搐觸處揣傳瘡闖創錘純蠢戳綽疵茨磁雌辭慈瓷詞賜聰蔥囪從叢湊粗醋簇促躥篡竄摧瘁粹淬磋撮搓措挫錯搭達傣戴帶貸擔單鄲撣膽氮但憚淡誕彈蛋當擋黨蕩檔搗蹈島禱導盜蹬燈鄧滴迪敵滌翟遞締顛掂滇碘點靛墊電惦奠澱殿釣調跌爹碟蝶叠諜疊釘頂錠訂東董懂動棟凍鬥犢獨讀堵睹賭鍍鍛斷緞堆兌隊對墩噸蹲敦頓囤鈍盾遁掇哆奪垛鵝額訛娥惡扼遏鄂餓恩兒爾餌洱貳發罰閥琺藩礬釩煩範販飯訪紡飛廢沸費紛墳奮憤糞豐楓蜂峰鋒風瘋烽逢馮縫諷鳳膚孵輻撫輔賦複傅腹負富訃婦縛咐噶嘎該改概鈣蓋溉幹趕感稈敢贛岡剛鋼綱崗篙臯糕搞鎬擱鴿胳疙閣隔鉻個給耕羹埂耿梗龔宮鞏貢鈎溝構購夠箍蠱顧剮關觀管館慣貫廣瑰規矽歸龜閨軌詭櫃跪貴劊輥滾棍鍋郭國裹過哈骸駭韓漢閡涸赫褐鶴賀嘿橫轟鴻紅後壺葫護滬戶嘩華畫劃話槐徊懷淮壞歡環桓還緩換喚瘓豢煥渙黃磺蝗簧謊揮輝徽恢蛔毀悔慧惠晦賄穢會燴彙諱誨繪葷渾豁夥獲霍貨禍擊機畸稽積箕饑激譏雞姬績緝極棘輯級擠幾脊薊冀劑悸濟計記既際繼紀枷夾莢頰賈鉀假稼價駕嫁殲監堅箋間艱緘繭檢柬堿鹼揀撿簡儉剪減薦檻鑒踐賤見鍵艦劍餞漸濺澗漿疆蔣槳獎講醬蕉椒礁焦膠澆驕嬌攪鉸矯僥腳餃繳絞剿教酵轎較窖揭稭階截節莖驚粳經警景頸靜境敬鏡徑痙靖竟競淨糾廄救舊駒舉據鋸懼劇捐鵑絹撅攫傑捷睫竭潔結戒藉芥誡屆緊錦僅謹進靳晉燼浸盡勁荊兢覺決訣絕鈞軍峻俊竣浚郡駿開揩凱慨堪勘坎砍康慷糠磕顆殼咳課墾懇摳庫褲誇塊儈寬礦曠況虧巋窺葵奎魁傀饋愧潰坤擴廓闊喇蠟臘萊來賴藍婪欄攔籃闌蘭瀾讕攬覽懶纜爛濫廊撈勞烙澇勒樂鐳壘擂肋類淚楞冷厘梨犁黎籬狸離漓理裏鯉禮莉荔麗厲勵礫曆傈痢粒瀝隸璃哩倆聯蓮連鐮廉憐漣簾斂臉鏈戀煉練糧涼兩輛量諒撩聊僚療燎寥遼潦撂鐐獵霖臨鄰鱗淋凜賃拎菱零齡鈴淩靈陵嶺領餾劉龍聾嚨籠窿隆壟攏隴樓婁摟簍漏陋蘆盧顱廬爐擄鹵虜魯賂祿錄陸戮驢呂鋁侶旅履屢縷慮率濾綠巒攣孿灤亂掄輪倫侖淪綸論蘿螺羅邏鑼籮騾裸落洛駱絡媽瑪碼螞馬罵嘛嗎埋買麥賣邁脈瞞饅蠻滿蔓謾貓錨鉚貿麽黴沒媒鎂門悶們萌蒙檬盟錳猛夢謎彌秘覓綿冕勉娩緬瞄藐渺廟蔑滅憫閩螟鳴銘謬摸摹蘑謀畝姆鈉納難囊撓腦惱鬧淖呢餒膩溺蔫攆撚娘釀鳥捏聶孽齧鑷鎳涅檸獰凝甯擰濘鈕紐膿濃農瘧挪懦糯諾哦歐鷗毆藕嘔偶漚攀潘盤磐盼畔龐中國昆山博愛天下耪賠噴抨鵬騙飄頻貧聘蘋萍憑瓶評屏坡潑頗撲鋪樸譜臍齊騎豈啓契砌氣棄訖掐牽扡釺鉛遷簽謙乾黔錢鉗潛遣淺譴塹嵌歉槍嗆腔羌牆薔強搶橇鍬橋喬僑鞘撬翹峭竅竊欽親輕氫傾卿頃請慶瓊窮趨區軀驅渠取娶齲趣顴權醛痊勸缺炔瘸卻鵲讓饒擾繞惹熱韌認紉榮絨揉褥軟銳閏潤弱撒灑薩腮鰓塞賽傘桑嗓喪搔騷掃澀殺紗傻啥煞篩曬閃陝擅贍繕墒傷賞梢捎稍燒紹奢賒蛇赦攝懾涉設砷紳審嬸腎滲聲繩勝聖師獅濕詩屍時蝕實識駛勢釋飾視試壽瘦獸蔬樞輸書贖孰熟薯暑曙署蜀黍鼠屬術樹豎數漱帥雙誰稅吮瞬順舜說碩爍絲嗣飼聳慫頌訟誦搜艘擻嗽蘇訴肅酸蒜雖綏髓碎歲孫損筍蓑梭唆縮瑣索鎖獺撻蹋擡泰酞攤貪癱灘壇檀痰潭譚談毯袒碳探歎湯糖燙濤滔縧騰疼謄銻題蹄啼體替嚏惕涕剃屜條眺貼鐵帖廳聽烴銅統頭圖塗團頹腿蛻褪退臀拖脫鴕馱駝橢窪襪豌彎灣頑萬網韋違桅圍唯惟爲濰維葦萎偉僞緯謂慰衛溫聞紋吻穩紊問嗡翁甕撾蝸渦窩斡握嗚鎢烏誣無蕪吳塢霧務誤錫犧稀膝犀檄襲習媳銑戲細蝦轄峽俠狹廈鍁鮮纖鹹賢銜舷閑顯險現獻縣腺餡羨憲陷限線廂鑲鄉詳響項蕭霄銷曉嘯楔些歇蠍鞋協挾攜脅諧寫械卸蟹懈泄瀉謝鋅釁興洶鏽袖繡墟戌需虛噓須徐許蓄緒續軒懸選癬眩絢靴薛學勳詢尋馴訓訊遜壓鴉鴨啞亞訝焉咽閹煙淹鹽嚴顔閻豔厭硯雁唁彥諺驗鴦楊揚佯瘍陽癢養樣漾邀腰瑤搖堯遙窯謠姚藥椰噎爺頁掖業葉腋夜液壹醫揖銥頤夷遺儀疑彜蟻藝億臆逸肄疫裔毅憶義詣議誼譯異翼翌繹蔭殷陰銀飲櫻嬰鷹應纓瑩螢營熒蠅穎硬喲擁傭臃癰庸雍踴蛹詠湧優憂郵鈾猶遊釉誘淤盂虞愚輿逾魚愉渝漁隅娛與嶼禹語籲峪禦獄譽預豫馭鴛淵轅園員圓猿源緣遠苑願怨院約越躍鑰嶽粵悅閱雲鄖勻隕運蘊醞暈韻砸雜災載攢暫贊贓髒葬遭糟鑿藻棗竈燥責擇則澤賊贈紮劄軋鍘閘詐齋債寨瞻氈盞斬輾嶄展蘸棧戰站湛綻張漲帳賬脹趙蟄轍鍺蔗這斟甄砧臻貞針偵診震振鎮陣蒸掙睜猙幀鄭證織職植殖執紙摯擲幟質鍾終種腫衆洲謅粥軸皺宙晝驟豬諸誅燭煮矚囑貯鑄築駐拽專磚轉撰賺篆樁莊裝妝撞壯狀錐贅墜綴諄著濁茲資滓漬鬃棕蹤宗綜總縱鄒揍詛組鑽纂緻鐘麼為隻兇準啟闆裡靂餘鍊洩";

        /// <summary>
        /// 转简体,字库少
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToChineseGB2312(this string value)
        {
            char[] charvalues = value.ToCharArray();
            int position;//字符位置，简繁体位置要相同
            foreach (char c in charvalues)
            {
                position = TraditionalChinese.IndexOf(c);
                if (position > 0)
                {
                    value = value.Replace(c, SimplifiedChinese[position]);
                }
            }
            return value;
        }

        /// <summary>
        /// 转繁体,字库少
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToChineseBIG5(this string value)
        {
            char[] charvalues = value.ToCharArray();
            int position;//字符位置，简繁体位置要相同
            foreach (char c in charvalues)
            {
                position = SimplifiedChinese.IndexOf(c);
                if (position > 0)
                {
                    value = value.Replace(c, TraditionalChinese[position]);
                }
            }
            return value;
        }

        /// <summary>
        /// 正则是否成功
        /// 不区分大小写
        /// </summary>
        /// <param name="value"></param>
        /// <param name="pattern"></param>
        /// <returns></returns>
        public static bool IsMatch(this string value, string pattern)
        {
            if (value == null)
            {
                return false;
            }
            else
            {
                Regex reg = new Regex(pattern, RegexOptions.IgnoreCase);
                return reg.IsMatch(value);
            }
        }

        /// <summary>
        /// 正则匹配
        /// 不区分大小写
        /// </summary>
        /// <param name="value"></param>
        /// <param name="pattern"></param>
        /// <returns></returns>
        public static string Match(this string value, string pattern)
        {
            if (value == null)
            {
                return string.Empty;
            }
            else
            {
                Regex reg = new Regex(pattern, RegexOptions.IgnoreCase);
                return reg.Match(value).Value;
            }
        }

        /// <summary>
        /// 正则按组匹配,失败返回null,成功返回SortedList
        /// 不区分大小写
        /// </summary>
        /// <param name="value"></param>
        /// <param name="pattern"></param>
        /// <returns></returns>
        public static SortedList<int, string> Matches(this string value, string pattern)
        {
            SortedList<int, string> list = null;
            if (value == null)
            {
                return null;
            }
            else
            {
                Regex reg = new Regex(pattern, RegexOptions.IgnoreCase | RegexOptions.ExplicitCapture);
                MatchCollection mc = reg.Matches(value);
                string[] groupNames = reg.GetGroupNames();
                string group = "[{0},{1}]";
                for (int i = 0; i < mc.Count; i++)
                {
                    Match item = mc[i];
                    if (item.Success)
                    {
                        if (list == null)
                        {
                            list = new SortedList<int, string>();
                        }

                        string groupvalue = "";
                        foreach (string groupName in groupNames)
                        {
                            groupvalue = string.Format(group, groupName, item.Groups[groupName].Value);
                        }
                        list.Add(i, groupvalue);
                    }
                }
            }
            return list;
        }

        /// <summary>
        /// 如果value 为 null 或空字符串 ("")，则为 true；否则为 false。
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static bool IsNullOrEmpty(this string value)
        {
            return string.IsNullOrEmpty(value);
        }

        /// <summary>
        /// 计算字符串的MD5。若字符串为空，则返回空，否则返回计算结果。  
        /// 用ASCII编码获取字符串
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string GetMD5Hash(this string value)
        {
            string hash = value;
            if (value != null)
            {
                System.Security.Cryptography.MD5CryptoServiceProvider md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
                byte[] data = Encoding.ASCII.GetBytes(value);
                data = md5.ComputeHash(data);
                hash = "";
                for (int i = 0; i < data.Length; i++)
                {
                    hash += data[i].ToString("X2");
                }
            }
            return hash;
        }

        /// <summary>
        /// 普通字符串转XML格式字符串
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ToXMLString(this string value)
        {
            value = value.Replace("&", "&amp;");
            value = value.Replace("<", "&lt;");
            value = value.Replace(">", "&gt;");
            value = value.Replace("'", "&apos;");
            value = value.Replace("\"", "&quot;");
            return value;
        }

        /// <summary>
        ///  XML格式字符串转普通字符串
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ToStringXML(this string value)
        {
            value = value.Replace("&amp;", "&");
            value = value.Replace("&lt;", "<");
            value = value.Replace("&gt;", ">");
            value = value.Replace("&apos;", "'");
            value = value.Replace("&quot;", "\\");
            return value;
        }

        /// <summary>
        /// 替换字符串中路径不被允许的字符
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string GetValidPath(this string path)
        {
            path = path.Replace("/", "");
            path = path.Replace("\\", "");
            path = path.Replace(":", "");
            path = path.Replace("*", "");
            path = path.Replace("?", "");
            path = path.Replace("\"", "");
            path = path.Replace("<", "");
            path = path.Replace(">", "");
            path = path.Replace("|", "");
            return path;
        }

        /// <summary>
        /// 过滤替换SQL不能出现的字符
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string FilterBySQL(this string str)
        {
            if (str == String.Empty)
            {
                return String.Empty;
            }
            //单引号替换成两个单引号
            str = str.Replace("'", "''");
            //半角封号替换为全角封号，防止多语句执行
            str = str.Replace(";", "；");
            //半角括号替换为全角括号
            str = str.Replace("(", "（");
            str = str.Replace(")", "）");
            ///////////////要用正则表达式替换，防止字母大小写得情况////////////////////
            //去除执行存储过程的命令关键字
            str = str.Replace("Exec", "");
            str = str.Replace("Execute", "");
            //去除系统存储过程或扩展存储过程关键字
            str = str.Replace("xp_", "x p_");
            str = str.Replace("sp_", "s p_");
            //防止16进制注入
            str = str.Replace("0x", "0 x");
            return str;
        }
    }
}
