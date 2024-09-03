using UnityEngine;

namespace Runtime {
    public class ParallaxBackground : MonoBehaviour {
        [SerializeField] Vector2 parallaxEffectMult;
        Transform cameraTransform;
        Vector3 lastCameraPosition;
        float textureUnitSizeX;
        float textureUnitSizeY;

        // Start is called before the first frame update
        protected void Start() {
            cameraTransform = Camera.main.transform;
            lastCameraPosition = cameraTransform.position;
            var sprite = GetComponent<SpriteRenderer>().sprite;
            var texture = sprite.texture;
            textureUnitSizeX = texture.width / sprite.pixelsPerUnit;
            textureUnitSizeY = texture.height / sprite.pixelsPerUnit;
        }

        // Update is called once per frame
        protected void LateUpdate() {
            var deltaMovement = cameraTransform.position - lastCameraPosition;
            transform.position += new Vector3(deltaMovement.x * parallaxEffectMult.x, deltaMovement.y * parallaxEffectMult.y);
            lastCameraPosition = cameraTransform.position;

            if (Mathf.Abs(cameraTransform.position.x - transform.position.x) >= textureUnitSizeX) {
                float offsetPostitionX = (cameraTransform.position.x - transform.position.x) % textureUnitSizeX;
                transform.position = new Vector3(cameraTransform.position.x + offsetPostitionX, transform.position.y, transform.position.z);
            }

            if (Mathf.Abs(cameraTransform.position.y - transform.position.y) >= textureUnitSizeY) {
                float offsetPostitionY = (cameraTransform.position.y - transform.position.y) % textureUnitSizeY;
                transform.position = new Vector3(transform.position.x, cameraTransform.position.y + offsetPostitionY, transform.position.z);
            }
        }
    }
}