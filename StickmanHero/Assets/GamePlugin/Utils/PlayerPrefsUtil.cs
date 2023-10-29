using System;
using UnityEngine;
using UnityEngine.Rendering;
using Random = UnityEngine.Random;

namespace mygame.sdk
{
    public class PlayerPrefsUtil
    {
        public static int cfBossHealth
        {
            get { return PlayerPrefsBase.Instance().getInt("cf_boss_health", 500); }
            set { PlayerPrefsBase.Instance().setInt("cf_boss_health", value); }
        }
        public static int cfBossDamage
        {
            get { return PlayerPrefsBase.Instance().getInt("cf_boss_damage", 10); }
            set { PlayerPrefsBase.Instance().setInt("cf_boss_damage", value); }
        }
        public static string Yesterday
        {
            get { return PlayerPrefsBase.Instance().getString("sf_yesterday", ""); }
            set { PlayerPrefsBase.Instance().setString("sf_yesterday", value); }
        }
        public static int enableZoom
        {
            get { return PlayerPrefsBase.Instance().getInt("apply_zoom_setting", 1); }
            set { PlayerPrefsBase.Instance().setInt("apply_zoom_setting", value); }
        }
        public static int levelShowRating
        {
            get { return PlayerPrefsBase.Instance().getInt("level_show_rate", 1); }
            set { PlayerPrefsBase.Instance().setInt("level_show_rate", value); }
        }

        public static int isShowRate
        {
            get { return PlayerPrefsBase.Instance().getInt("is_show_rate", 0); }
            set { PlayerPrefsBase.Instance().setInt("is_show_rate", value); }
        }

        public static int SoundFxSetting
        {
            get { return PlayerPrefsBase.Instance().getInt("SoundFxSetting", 1); }
            set { PlayerPrefsBase.Instance().setInt("SoundFxSetting", value); }
        }

        public static int MusicSetting
        {
            get { return PlayerPrefsBase.Instance().getInt("MusicSetting", 1); }
            set { PlayerPrefsBase.Instance().setInt("MusicSetting", value); }
        }
        public static int BgMusicSetting
        {
            get { return PlayerPrefsBase.Instance().getInt("key_config_bgmusic", 0); }
            set { PlayerPrefsBase.Instance().setInt("key_config_bgmusic", value); }
        }

        public static int VibrateSetting
        {
            get { return PlayerPrefsBase.Instance().getInt("key_config_vibrate", 1); }
            set { PlayerPrefsBase.Instance().setInt("key_config_vibrate", value); }
        }

        public static int AudioSetting
        {
            get { return PlayerPrefsBase.Instance().getInt("audio_setting", 1); }
            set { PlayerPrefsBase.Instance().setInt("audio_setting", value); }
        }

        public static int Theme
        {
            get { return PlayerPrefsBase.Instance().getInt("game_res_theme", 0); }
            set
            {
                PlayerPrefsBase.Instance().setInt("game_res_theme", value);
            }
        }

    }
}