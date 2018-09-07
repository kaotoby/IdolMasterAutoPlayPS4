using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdolMasterAutoPlayPS4.Models
{
    public class Song
    {
        public string Name { get; private set; }
        public SongType SongType { get; private set; }
        public string ImageUri { get; private set; }
        public string ScriptPath { get; private set; }

        public static Song[] List { get { return _list; } }
        private static Song[] _list = {
            new Song("s01", "THE IDOLM@STER"),
            new Song("s02", "my song"),
            new Song("s03", "私はアイドル"),
            new Song("s04", "GO MY WAY!!"),
            new Song("s05", "キミ＊チャンネル"),
            new Song("s06", "ビジョナリー"),
            new Song("s07", "edeN"),
            new Song("s08", "Vault That Borderline!"),
            new Song("s09", "七彩ボタン"),
            new Song("s10", "キラメキラリ"),
            new Song("s11", "いっぱいいっぱい"),
            new Song("q01", "Happy!"),
            new Song("q02", "ザ・ライブ革命でSHOW!"),
            new Song("q03", "Honey Heartbeat"),
            new Song("q04", "READY!!"),
            new Song("q05", "CAHNGE!!!!"),
            new Song("q06", "自分REST@AT"),
            new Song("q07", "99 Night"),
            new Song("m01", "キラキラいっぱいWAY!!"),
            new Song("m02", "THE H@PPY LIVE"),
        };

        private Song(string id, string name) {
            Name = name;
            if (id[0] == 'q') {
                SongType = SongType.Quintet;
            } else if (id[0] == 'm') {
                SongType = SongType.Medley;
            } else {
                SongType = SongType.Normal;
            }
            ScriptPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "IMSS", "Songs", id + ".imss");
            ImageUri = "/Resources/Images/" + id + ".png";
        }
    }

    public enum SongType
    {
        Normal,
        Quintet,
        Medley
    }
}
