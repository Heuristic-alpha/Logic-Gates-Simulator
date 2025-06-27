using UnityEngine;

namespace HSCL
{
    /// <summary>
    /// Cooldown store time. return true if current time exceeding waiting time, else return false.
    /// </summary>
    public class Cooldown
    {
        public readonly float waitTime;
        private float _lastTime;

        public Cooldown(float waitTime)
        {
            this.waitTime = waitTime;
            Restart();
        }

        /// <summary>
        /// Start = Restart
        /// </summary>
        public void Restart()
        {
            _lastTime = Time.time;
        }

        /// <summary>
        /// Is Cooldown complete?
        /// </summary>
        /// <returns></returns>
        public bool IsComplete()
        {
            return Time.time - _lastTime > waitTime;
        }

        /// <summary>
        /// Return how much time exceeding from last waiting Time.
        /// </summary>
        public float Laps()
        {
            return Time.time - _lastTime;
        }

        /// <summary>
        /// Syntax SUGAR for automaticly check and trigger Restart() after completion.
        /// </summary>
        /// <returns>Return true if already completed then Restart()</returns>
        public bool Auto()
        {
            bool answer = IsComplete();
            if (answer)
            {
                Restart();
                return true;
            }
            else return false;
        }

        public static implicit operator bool(Cooldown cooldown)
        {
            return cooldown.IsComplete();
        }      
    }
}
