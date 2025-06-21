using System.Threading.Tasks;
using UnityEngine;

namespace ColourMatch
{
	/// <summary>
	/// GameCamera which provides utility functions for converting between screen and world space. 
	/// </summary>
	[RequireComponent(typeof(Camera))]
	public class GameCamera : MonoBehaviour
	{
		/// <summary>
		/// The camera component being used in game.
		/// </summary>
		private Camera theCamera;

		/// <summary>
		/// Cached value of the half width of the orthographic camera calculated from the half height.
		/// </summary>
		private float cameraHalfWidth;

		/// <summary>
		/// Cached value of the half height of the orthographic camera.
		/// </summary>
		private float cameraHalfHeight;

		/// <summary>
		/// Cached value of half the screen width.
		/// </summary>
		private float halfScreenWidth;

		/// <summary>
		/// Cached value of half the screen height.
		/// </summary>
		private float halfScreenHeight;

		public Task InitialiseAsync()
		{
			Initialise();
			return Task.CompletedTask;
		}

		private void Initialise()
		{
			// Calculate the cached values we'll need to convert between screen and world space.
			theCamera = gameObject.GetComponent<Camera>();
			cameraHalfHeight = theCamera.orthographicSize;
			cameraHalfWidth = theCamera.aspect * cameraHalfHeight;
			halfScreenWidth = Screen.width * 0.5f;
			halfScreenHeight = Screen.height * 0.5f;
		}

		/// <summary>
		/// Convert between screen space and world space. Screen space is defined as the bottom left of the screen being (0,0) and the top right being (Screen.width, Screen.height).
		/// </summary>
		/// <param name="screenPosition">The position in screen space.</param>
		/// <returns>A world space position, on the Z=0.0f plane.</returns>
		public Vector3 ScreenPositionToWorldPosition(Vector2 screenPosition)
		{
			return new Vector3(
				(screenPosition.x - halfScreenWidth) / halfScreenWidth * cameraHalfWidth,
				(screenPosition.y - halfScreenHeight) / halfScreenHeight * cameraHalfHeight,
				0.0f
			);
		}

		/// <summary>
		/// Convert between world space and screen space. Screen space is defined as the bottom left of the screen being (0,0) and the top right being (Screen.width, Screen.height).
		/// </summary>
		/// <param name="worldPosition">A position in world space. The Z component is ignored.</param>
		/// <returns>A screen space position.</returns>
		public Vector2 WorldPositionToScreenPosition(Vector3 worldPosition)
		{
			return new Vector2(
				(worldPosition.x / cameraHalfWidth) * halfScreenWidth + halfScreenWidth,
				(worldPosition.y / cameraHalfHeight) * halfScreenHeight + halfScreenHeight
			);
		}
	}
}