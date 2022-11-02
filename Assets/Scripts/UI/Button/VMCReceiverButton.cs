using UnityEngine;
using TMPro;
using UnityEngine.UI;
using uOSC;
using System;
using VRM;

namespace HardCoded.VRigUnity {
	public class VMCReceiverButton : MonoBehaviour {
		public VMCReceiver vmcReceiver;

		[SerializeField] private TMP_Text buttonText;
		[SerializeField] private TMP_Text portText;
		private Button toggleButton;
		private Image buttonImage;
		private bool isVMCStarted;

		[SerializeField] Color toggleOnColor  = new(0.08009967f, 0.6792453f, 0.3454931f); // 0x14AD58
		[SerializeField] Color toggleOffColor = new(0.6981132f, 0, 0.03523935f); // 0xB30009
		
		void Start() {
			buttonImage = GetComponent<Image>();
			toggleButton = GetComponent<Button>();

			InitializeContents();
		}

		private void InitializeContents() {
			buttonImage.color = toggleOnColor;
			isVMCStarted = false;

			// Setup settings listener (TODO: Remove)
			Settings.VMCReceiverPortListener += (value) => {
				// Only display port changes when the VMC is closed
				if (!isVMCStarted) {
					portText.text = "Port " + value;
				}
			};

			portText.text = "Port " + Settings.VMCReceiverPort;
			
			toggleButton.onClick.RemoveAllListeners();
			toggleButton.onClick.AddListener(delegate {
				SetVMC(!isVMCStarted);
			});
		}

		private void SetVMC(bool enable) {
			isVMCStarted = enable;
			buttonImage.color = enable ? toggleOffColor : toggleOnColor;
			buttonText.text = enable ? "Stop Receiver VMC" : "Start Receiver VMC";

			// Start/Stop the VMC instance
			if (enable) {
				vmcReceiver.SetPort(Settings.VMCReceiverPort);
				vmcReceiver.StartVMC();
			} else {
				vmcReceiver.StopVMC();
			}

			// Update port
			portText.text = "Port " + Settings.VMCReceiverPort;
		}
	}
}
