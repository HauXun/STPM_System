import { Backdrop, Box, Fade, Modal, ModalProps } from '@mui/material';

type Props = ModalProps & {
  open: boolean;
};

const style = {
  position: 'absolute',
  top: '50%',
  left: '50%',
  transform: 'translate(-50%, -50%)',
  width: 'max-content',
};

export default function Popup({ children, onClose, open }: Props) {
  return (
    <Modal
      keepMounted
      aria-labelledby="keep-mounted-transition-modal-title"
      aria-describedby="keep-mounted-transition-modal-description"
      open={open}
      onClose={onClose}
      closeAfterTransition
      slots={{ backdrop: Backdrop }}
      slotProps={{
        backdrop: {
          timeout: 500,
        },
      }}
    >
      <Fade in={open}>
        <Box sx={style}>{children}</Box>
      </Fade>
    </Modal>
  );
}
