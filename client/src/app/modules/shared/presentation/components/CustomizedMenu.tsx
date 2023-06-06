import { CheckRounded } from '@mui/icons-material';
import ArrowDropDownIcon from '@mui/icons-material/ArrowDropDown';
import {
  Divider,
  IconButton,
  ListItemIcon,
  ListItemText,
  Menu,
  MenuItem,
  MenuList,
  Paper,
  Typography,
  darken,
} from '@mui/material';
import { makeStyles } from '@mui/styles';
import { useState } from 'react';

const useStyles = makeStyles((theme) => ({
  btnHover: {
    backgroundColor: theme.palette.primary.main,
    '&:hover': {
      backgroundColor: darken(theme.palette.primary.main, 0.2),
    },
  },
}));

type Props = {
  preText: string;
};

const CustomizedMenu = ({ preText }: Props) => {
  const classes = useStyles();
  const [anchorEl, setAnchorEl] = useState<null | HTMLElement>(null);
  const open = Boolean(anchorEl);
  const handleClick = (event: React.MouseEvent<HTMLButtonElement>) => {
    setAnchorEl(event.currentTarget);
  };
  const handleClose = () => {
    setAnchorEl(null);
  };

  return (
    <>
      <span
        className={`${classes.btnHover} inline-flex cursor-pointer select-none items-center rounded-lg p-0 text-white transition active:bg-green-800`}
      >
        <Typography sx={{ px: 2, fontSize: '1rem', fontWeight: 600, lineHeight: '1rem' }}>
          {preText}
        </Typography>
        <Divider className="bg-white" orientation="vertical" flexItem variant="middle" />
        <IconButton
          sx={{ p: 0, borderRadius: 0, width: 40, height: 40 }}
          aria-controls="dropdown-menu"
          aria-haspopup="true"
          color="inherit"
          onClick={handleClick}
        >
          <ArrowDropDownIcon />
        </IconButton>
      </span>
      <Menu
        id="basic-menu"
        anchorEl={anchorEl}
        open={open}
        onClose={handleClose}
        MenuListProps={{
          'aria-labelledby': 'basic-button',
        }}
      >
        <Paper elevation={0} sx={{ width: 400, maxWidth: '100%' }}>
          <MenuList>
            <MenuItem onClick={handleClose}>
              <ListItemIcon>
                <CheckRounded fontSize="small" />
              </ListItemIcon>
              <ListItemText
                primary="Huỷ đề tài"
                secondary="Huỷ đề tài vì một lý do nào đó mà nhóm không tham gia nữa hoặc các vấn đề phát sinh"
                sx={{ whiteSpace: 'break-spaces' }}
              />
            </MenuItem>
            <MenuItem onClick={handleClose}>
              <ListItemIcon>{/* <CheckRounded fontSize="small" /> */}</ListItemIcon>
              <ListItemText
                primary="Bắt buộc huỷ"
                secondary="Bắt buộc huỷ đề tài vì vi phạm hoặc các vấn đề phát sinh"
                sx={{ whiteSpace: 'break-spaces' }}
              />
            </MenuItem>
          </MenuList>
        </Paper>
      </Menu>
    </>
  );
};

export default CustomizedMenu;
