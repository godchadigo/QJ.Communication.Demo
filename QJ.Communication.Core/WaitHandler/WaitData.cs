using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TouchSocket.Core;

namespace QJ.Communication.Core.WaitHandler
{

    /// <summary>
    /// 等待数据对象
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class WaitData<T> : DisposableObject
    {
        private readonly AutoResetEvent m_waitHandle;
        private volatile WaitDataStatus m_status;
        private CancellationTokenRegistration m_tokenRegistration;

        /// <summary>
        /// WaitData
        /// </summary>
        public WaitData()
        {
            this.m_waitHandle = new AutoResetEvent(false);
        }

        /// <inheritdoc/>
        public WaitDataStatus Status => this.m_status;

        /// <inheritdoc/>
        public T WaitResult { get; private set; }

        /// <inheritdoc/>
        public void Cancel()
        {
            this.m_status = WaitDataStatus.Canceled;
            this.m_waitHandle.Set();
        }

        /// <inheritdoc/>
        public void Reset()
        {
            if (this.m_tokenRegistration != default)
            {
                this.m_tokenRegistration.Dispose();
            }
            this.m_status = WaitDataStatus.Default;
            this.WaitResult = default;
            this.m_waitHandle.Reset();
        }

        /// <inheritdoc/>
        public bool Set()
        {
            this.m_status = WaitDataStatus.Success;
            return this.m_waitHandle.Set();
        }

        /// <inheritdoc/>
        public bool Set(T waitResult)
        {
            this.WaitResult = waitResult;
            this.m_status = WaitDataStatus.Success;
            return this.m_waitHandle.Set();
        }

        /// <inheritdoc/>
        public void SetCancellationToken(CancellationToken cancellationToken)
        {
            if (cancellationToken.CanBeCanceled)
            {
                if (this.m_tokenRegistration == default)
                {
                    this.m_tokenRegistration = cancellationToken.Register(this.Cancel);
                }
                else
                {
                    this.m_tokenRegistration.Dispose();
                    this.m_tokenRegistration = cancellationToken.Register(this.Cancel);
                }
            }
        }

        /// <inheritdoc/>
        public void SetResult(T result)
        {
            this.WaitResult = result;
        }

        /// <inheritdoc/>
        public WaitDataStatus Wait(TimeSpan timeSpan)
        {
            return this.Wait((int)timeSpan.TotalMilliseconds);
        }

        /// <inheritdoc/>
        public WaitDataStatus Wait(int millisecond)
        {
            if (!this.m_waitHandle.WaitOne(millisecond))
            {
                this.m_status = WaitDataStatus.Overtime;
            }
            return this.m_status;
        }

        /// <inheritdoc/>
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                this.m_status = WaitDataStatus.Disposed;
                this.WaitResult = default;
                this.m_waitHandle.SafeDispose();
                this.m_tokenRegistration.Dispose();
            }
            base.Dispose(disposing);
        }
    }

    /// <summary>
    /// 等待数据对象
    /// </summary>
    public class WaitData : WaitData<object>
    {
    }
}
