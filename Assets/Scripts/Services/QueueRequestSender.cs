using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.Networking;

namespace Services
{
    public interface IRequestSender
    {
        UniTask<T> SendRequest<T>(UnityWebRequest request, CancellationTokenSource cts, Func<UnityWebRequest, T> resultExtractor);
        void ClearCanceledRequests();
    }
    
    public class QueueRequestSender : IRequestSender
    {
        private readonly List<QueueRequestElement> _requestsForSend = new();
        private bool _isBusy;

        public async UniTask<T> SendRequest<T>(UnityWebRequest request, CancellationTokenSource cts, Func<UnityWebRequest, T> resultExtractor)
        {
            if (_isBusy)
                return await AwaitQueue(new QueueRequestElement<T>(request, cts, resultExtractor));
            
            _isBusy = true;

            try
            {
                await request.SendWebRequest().ToUniTask(cancellationToken: cts.Token);
                
                if (request.result == UnityWebRequest.Result.Success)
                    return resultExtractor.Invoke(request);

                Debug.LogError($"{GetType().Name} request error. result {request.result}, responseCode {request.responseCode}");
                return default;
            }
            catch (OperationCanceledException)
            {
                return default;
            }
            catch (Exception e)
            {
                Debug.LogError($"{GetType().Name} SendRequest error. message {e.Message}, StackTrace {e.StackTrace}");
                return default;
            }
            finally
            {
                _isBusy = false;
                request.Dispose();
                TrySendNextRequest();
            }
        }

        private void TrySendNextRequest()
        {
            if (_requestsForSend.Count <= 0) 
                return;
            
            for (var i = _requestsForSend.Count - 1; i >= 0; i--)
            {
                var element = _requestsForSend[i];
                _requestsForSend.RemoveAt(i);
                var isRequestActual = !element.IsCanceled;
                element.Tcs.SetResult(isRequestActual);
                
                if (isRequestActual)
                    break;
            }
        }

        public void ClearCanceledRequests()
        {
            for (var i = _requestsForSend.Count - 1; i >= 0; i--)
            {
                if (!_requestsForSend[i].IsCanceled)
                    continue;
                
                _requestsForSend.RemoveAt(i);
            }
        }

        private async UniTask<T> AwaitQueue<T>(QueueRequestElement<T> element) 
        {
            _requestsForSend.Add(element);
            await element.Tcs.Task;

            if (!element.Tcs.Task.Result)
                return default;
            
            return await element.SendRequest(SendRequest);
        }
    }
    
    public class QueueRequestElement
    {
        protected UnityWebRequest Request { get; }
        protected CancellationTokenSource Cts { get; }
        public TaskCompletionSource<bool> Tcs { get; }
        public bool IsCanceled => Cts.IsCancellationRequested;

        protected QueueRequestElement(UnityWebRequest request, CancellationTokenSource cts)
        {
            Request = request;
            Cts = cts;
            Tcs = new TaskCompletionSource<bool>();
        }
    }
    
    public class QueueRequestElement<T> : QueueRequestElement
    {
        private readonly Func<UnityWebRequest, T> _resultExtractor;

        public QueueRequestElement(UnityWebRequest request, CancellationTokenSource cts, Func<UnityWebRequest, T> resultExtractor)
            : base(request, cts)
        {
            _resultExtractor = resultExtractor;
        }

        public async UniTask<T> SendRequest(Func<UnityWebRequest, CancellationTokenSource, Func<UnityWebRequest, T>, UniTask<T>> sendMethod)
        {
            return await sendMethod.Invoke(Request, Cts, _resultExtractor);
        }
    }
}