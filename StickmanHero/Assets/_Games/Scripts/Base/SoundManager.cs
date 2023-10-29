using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using mygame.sdk;

namespace SuperFight
{
    public enum TYPE_RAND_FX
    {
        FX_HIT = 0,
        FX_TAKE_DAMAGE,
        FX_SLASH,
        FX_SWORD_SLASH
    }
    public class SoundManager : MonoBehaviour
    {
        public AudioSource _soundBG;
        public AudioSource _soundEff;
        public AudioSource _soundEff2;
        [SerializeField] float volumeMusicBg = 0.7f;

        [Header("Sound General")]
        public AudioClip musicBgMenu;
        public AudioClip musicBgGameplay;
        public AudioClip musicBgGameBoss;
        public AudioClip effClickUI;
        public AudioClip effWin;
        public AudioClip effLose;
        public AudioClip effWinUI;
        public AudioClip effLoseUI;

        //MainPlayer---------------------------------------------------------------------------------------------
        [Header("Sound Main Player")]
        public AudioClip effWalk;
        public AudioClip effRun;
        public AudioClip effJump;
        public AudioClip effLanding;
        public AudioClip effDash;
        
        public AudioClip effHit1;
        public AudioClip effHit2;
        public AudioClip effHit3;
        public AudioClip effBeHit1;
        public AudioClip effBeHit2;
        public AudioClip effBeHit3;
        public AudioClip effBeHit4;
        public AudioClip effSwordSlash1;
        public AudioClip effSwordSlash2;
        public AudioClip effSwordSlash3;
        public AudioClip effSwordSlash4;

        public AudioClip effRevive;
        public AudioClip effDeath;

        
        //Enemy---------------------------------------------------------------------------------------------
        [Header("Sound Enemy")]
        public AudioClip effShootFireball;
        public AudioClip effExplode;
        public AudioClip effZombie1;
        public AudioClip effZombie2;
        public AudioClip effZombie3;
        public AudioClip effZombieDie;
        public AudioClip effCreeper;
        public AudioClip effEnderman;
        public AudioClip effPig;
        public AudioClip effPigDie;
        public AudioClip effSpider;
        public AudioClip effSpiderDie;
        public AudioClip effBossTitan;
        public AudioClip effBossTitanDie;
        public AudioClip effBossTitanRunning;
        public AudioClip effBossTitanBreak;
        public AudioClip effBossGhost;
        public AudioClip effBossDragon;
        public AudioClip effBossDie;
        public AudioClip effSlash1;
        public AudioClip effSlash2;
        //Item---------------------------------------------------------------------------------------------
        [Header("Sound ItemGame")]
        public AudioClip effSavePoint;
        public AudioClip effOpenDoor;
        public AudioClip effCollectCoin;
        public AudioClip effItemHealthTouch;
        public AudioClip effItemHealthBeEat;
        public AudioClip effWeaponCollect;
        public AudioClip effItemChangeSkin;
        public AudioClip effEatingApple;
        public AudioClip effChestOpenning;
        public AudioClip effChestBreak;
        public AudioClip effEatItemWeapon;

        //Trap-------------------------------------------------------------------------------------------------

        [Header("Other")]
        public AudioClip effCoinUI;
        public AudioClip effUpgrade;
        public AudioClip effSpin;
        public AudioClip effStarKick;
        public AudioClip effFlipCard;
        public AudioClip effGetNewSkin;

        //-------------------------------------------------------------------------------------------------------
        List<AudioSource> mListAudioPrivate;
        public bool IsEnable { set; get; }
        public bool IsEnableMusic { set; get; }
        public bool IsEnableEffect { set; get; }

        public static SoundManager Instance = null;

        void Awake()
        {
            Instance = this;

            mListAudioPrivate = new List<AudioSource>();

            IsEnable = PlayerPrefsUtil.AudioSetting > 0 ? true : false;
            IsEnableEffect = PlayerPrefsUtil.SoundFxSetting > 0 ? true : false;
            IsEnableMusic = PlayerPrefsUtil.MusicSetting > 0 ? true : false;

            if (!IsEnable)
            {
                IsEnableEffect = IsEnable;
                IsEnableMusic = IsEnable;
            }

            setEnableEffect(IsEnableEffect);
            setEnableMusic(IsEnableMusic);
        }


        public void playSoundBG(AudioClip bg)
        {
            if (bg == null)
            {
                return;
            }
            else if (_soundBG.clip != bg)
            {
                _soundBG.clip = bg;
            }

            _soundBG.Play();
        }

        public void stopSoundBG()
        {
            _soundBG.Stop();
        }

        public void stopSoundEff()
        {
            _soundEff.Stop();
            stopSoundPrivate();
        }

        // public void playSoundFx(AudioClip eff)
        // {
        //     _soundEff.clip = eff;
        //     _soundEff.Play();
        // }
        // public void stopSoundFx(AudioClip eff)
        // {
        //     _soundEff.Pause();
        //     _soundEff.clip = null;
        //     _soundEff.Play();
        // }

        public void playSoundFx(AudioClip eff)
        {
            _soundEff.PlayOneShot(eff);
        }



        //Sound Private-----------------------------------------------------------------------------------------------------------------

        // public void stopSoundPrivate(int idx)
        // {
        //     if (idx >= 0 && idx < mListAudioPrivate.Count)
        //     {
        //         mListAudioPrivate[idx].Stop();
        //     }

        // }

        public AudioSource playSoundPrivate(AudioClip clipAudio, bool isLoop, float delay = 0, float vol = 0)
        {
            AudioSource audioSource = null;
            // int idx = 0;
            bool isCreateNew = true;

            for (int i = 0; i < mListAudioPrivate.Count; ++i)
            {
                audioSource = mListAudioPrivate[i];
                if (!audioSource.isPlaying)
                {
                    // idx = i;
                    isCreateNew = false;
                    break;
                }
            }

            if (isCreateNew)
            {
                audioSource = gameObject.AddComponent<AudioSource>();
                mListAudioPrivate.Add(audioSource);
            }

            audioSource.loop = isLoop;
            audioSource.volume = _soundEff.volume;
            if (vol > 0 && IsEnableEffect)
            {
                audioSource.volume = vol;
            }

            audioSource.clip = clipAudio;
            if (delay > 0)
            {
                audioSource.PlayDelayed(delay);
            }
            else
            {
                audioSource.Play();
            }

            return audioSource;
        }

        //--------------------------------------------------------------------------------------------------------------------

        public void enableSoundInAds(bool isActive)
        {
            if (IsEnableEffect)
            {
                if (isActive)
                {
                    AudioListener.volume = 1.0f;
                }
                else
                {
                    AudioListener.volume = 0.0f;
                }
            }
            else if (IsEnableMusic && _soundBG.isPlaying)
            {
                if (isActive)
                {
                    _soundBG.Play();
                }
                else
                {
                    _soundBG.Pause();
                }
            }
        }

        public void setEnableSound(bool enable)
        {
            if (enable)
            {
                AudioListener.volume = 1.0f;
            }
            else
            {
                AudioListener.volume = 0.0f;
            }
        }

        public void setEnableEffect(bool enable)
        {
            IsEnableEffect = enable;
            if (enable)
            {
                _soundEff.volume = 1.0f;
                setEnableSoundPrivate(1.0f);
            }
            else
            {
                _soundEff.volume = 0.0f;
                setEnableSoundPrivate(0.0f);
            }
        }

        public void stopSoundPrivate()
        {
            for (int i = 0; i < mListAudioPrivate.Count; ++i)
            {
                mListAudioPrivate[i].Stop();
            }
        }

        public void setEnableSoundPrivate(float vol)
        {
            for (int i = 0; i < mListAudioPrivate.Count; ++i)
            {
                mListAudioPrivate[i].volume = vol;
            }
        }

        public void setEnableMusic(bool enable)
        {
            IsEnableMusic = enable;
            if (enable)
            {
                _soundBG.volume = volumeMusicBg;
            }
            else
            {
                _soundBG.volume = 0.0f;
            }
        }

        public void soundOn_Off(bool enable)
        {
            IsEnable = enable;

            PlayerPrefsUtil.AudioSetting = IsEnable ? 1 : 0;
            setEnableEffect(IsEnable);
            setEnableMusic(IsEnable);
        }

        public void soundOn_Off()
        {
            if (IsEnable)
            {
                IsEnable = false;
            }
            else
            {
                IsEnable = true;
            }
            PlayerPrefsUtil.AudioSetting = IsEnable ? 1 : 0;
            setEnableEffect(IsEnable);
            setEnableMusic(IsEnable);
        }

        public void effectOn_Off()
        {
            if (IsEnableEffect)
            {
                IsEnableEffect = false;
            }
            else
            {
                IsEnableEffect = true;
            }
            PlayerPrefsUtil.SoundFxSetting = IsEnableEffect ? 1 : 0;
            setEnableEffect(IsEnableEffect);
        }

        public void musicOn_Off()
        {
            if (IsEnableMusic)
            {
                IsEnableMusic = false;
            }
            else
            {
                IsEnableMusic = true;
            }
            PlayerPrefsUtil.MusicSetting = IsEnableMusic ? 1 : 0;
            setEnableMusic(IsEnableMusic);
        }

        public void musicWait(float duration = -1)
        {
            if (!IsEnableMusic)
            {
                return;
            }
            _soundBG.Pause();
            if (duration > 0)
            {
                StartCoroutine(resumeMusic(duration));
            }
        }

        IEnumerator resumeMusic(float duration)
        {
            yield return new WaitForSeconds(duration);
            _soundBG.UnPause();
        }


        //------------------------------------------------------------------------------------------------------------------------------------------------
        public void playRandFx(TYPE_RAND_FX type)
        {
            if (!IsEnableEffect) return;
            List<AudioClip> listClip = null;
            switch (type)
            {
                case TYPE_RAND_FX.FX_HIT:
                    listClip = new List<AudioClip>() { effHit1, effHit2, effHit3 };
                    break;

                case TYPE_RAND_FX.FX_SLASH:
                    listClip = new List<AudioClip>() { effSlash1, effSlash2 };
                    break;
                case TYPE_RAND_FX.FX_SWORD_SLASH:
                    listClip = new List<AudioClip>() { effSwordSlash1, effSwordSlash2, effSwordSlash3, effSwordSlash4 };
                    break;

                default:
                    listClip = new List<AudioClip>() { effBeHit1, effBeHit2, effBeHit3, effBeHit4 };
                    break;
            }

            _soundEff2.PlayOneShot(listClip[Random.Range(0, listClip.Count)]);
        }

        public void playRandFx(AudioClip[] arrClip)
        {
            if (!IsEnableEffect) return;
            if (arrClip == null)
            {
                return;
            }
            else if (arrClip.Length < 1)
            {
                return;
            }

            _soundEff.PlayOneShot(arrClip[Random.Range(0, arrClip.Length)]);
        }
    }
}