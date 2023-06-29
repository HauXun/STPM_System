import { IonIcon } from '@ionic/react';
import { KeyboardArrowDownRounded, KeyboardArrowRightRounded } from '@mui/icons-material/';
import { TreeItem, TreeItemProps, TreeView, treeItemClasses } from '@mui/lab';
import { Box, CardMedia, Divider, Drawer, IconProps, Typography } from '@mui/material';
import { styled } from '@mui/material/styles';
import {
  bookmarksOutline,
  brushOutline,
  copyOutline,
  cubeOutline,
  documentTextOutline,
  funnelOutline,
  keyOutline,
  notificationsOutline,
  peopleOutline,
  settingsOutline,
  documentsOutline,
  albumsOutline
} from 'ionicons/icons';
import { useState } from 'react';
import { NavLink } from 'react-router-dom';
import { DRAWER_WIDTH } from '~/app/modules/shared/constants';
import { TopicRoutes } from '~/app/modules/topic/presentation/routes';
import { UserRoutes } from '~/app/modules/user/presentation/routes';
import logo from '~/main/assets/logo_app.png';
import { CustomScrollbar } from '../../CustomScrollbar';
import BoxFlexCenter from '~/app/modules/core/presentation/containers/BoxFlexCenter';

const CustomDrawer = styled(Drawer)(({ theme }) => ({
  '& .MuiDrawer-paper': {
    boxSizing: 'border-box',
    width: DRAWER_WIDTH,
    height: '100%',
    backgroundColor: theme.palette.background.default,
    overflowY: 'hidden',
  },
}));

const CustomDivider = styled(Divider)(({ theme }) => ({
  width: DRAWER_WIDTH - 50,
  margin: 'auto',
  transform: 'translateY(-5px)',
}));

type StyledTreeItemProps = TreeItemProps & {
  bgColor?: string;
  color?: string;
  labelIcon: React.ElementType<IconProps>;
  iconSize?: string;
  labelText: string;
  textSize?: string;
};

const StyledTreeItemRoot = styled(TreeItem)(({ theme }) => ({
  color: theme.palette.text.secondary,
  [`& .${treeItemClasses.content}`]: {
    color: theme.palette.text.secondary,
    borderTopRightRadius: theme.spacing(0),
    borderBottomRightRadius: theme.spacing(0),
    paddingRight: theme.spacing(1),
    paddingTop: theme.spacing(1),
    paddingBottom: theme.spacing(1),
    fontWeight: theme.typography.fontWeightMedium,
    '&.Mui-expanded': {
      fontWeight: theme.typography.fontWeightRegular,
      [`& .${treeItemClasses.iconContainer}, & .${treeItemClasses.label}`]: {
        color: 'var(--tree-view-color)',
        fontWeight: theme.typography.fontWeightBold,
      },
    },
    '&:hover': {
      backgroundColor: theme.palette.action.hover,
    },
    '&.Mui-focused, &.Mui-selected, &.Mui-selected.Mui-focused': {
      backgroundColor: 'unset',
      color: 'unset',
      fontWeight: 'unset',
    },
    [`& .${treeItemClasses.label}`]: {
      fontWeight: 'inherit',
      color: 'inherit',
    },
  },
  [`& .${treeItemClasses.group}`]: {
    marginLeft: 0,
    [`& .${treeItemClasses.content}`]: {
      paddingLeft: theme.spacing(2),
    },
  },
}));

function StyledTreeItem(props: StyledTreeItemProps) {
  const { bgColor, color, labelIcon: Icon, iconSize, textSize, labelText, ...other } = props;
  const setIconSize = iconSize ?? '1.25rem';
  const setTextSize = textSize ?? '1rem';

  const labelContent = (
    <BoxFlexCenter sx={{ p: 0.5, pr: 0, fontSize: setIconSize }}>
      <Box component={() => <Icon />} color="inherit" />
      <Typography
        variant="body2"
        sx={{
          fontWeight: 'inherit',
          fontFamily: 'Raleway',
          flexGrow: 1,
          fontSize: setTextSize,
          ml: 3,
        }}
      >
        {labelText}
      </Typography>
    </BoxFlexCenter>
  );

  // const wrappedLabel = href ? <NavLink end to={href}>{labelContent}</NavLink> : labelContent;

  return (
    <StyledTreeItemRoot
      label={labelContent}
      style={{
        '--tree-view-color': color,
        '--tree-view-bg-color': bgColor,
      }}
      {...other}
    />
  );
}

const StyledNavLink = styled(NavLink)(({ theme }) => ({
  '&.active': {
    [`& .${treeItemClasses.root}`]: {
      [`& .${treeItemClasses.content}`]: {
        backgroundColor: `var(--tree-view-bg-color, ${theme.palette.action.selected})`,
        color: 'var(--tree-view-color)',
        fontWeight: theme.typography.fontWeightBold,
        '&.Mui-focused, &.Mui-selected': {
          backgroundColor: `var(--tree-view-bg-color, ${theme.palette.action.selected})`,
          color: 'var(--tree-view-color)',
          fontWeight: theme.typography.fontWeightBold,
        },
      },
    },
  },
  '&': {
    [`& .${treeItemClasses.root}`]: {
      [`& .${treeItemClasses.content}`]: {
        '&.Mui-focused, &.Mui-selected': {
          backgroundColor: 'inherit',
          color: 'inherit',
        },
        '&.Mui-selected:hover': {
          backgroundColor: theme.palette.action.hover,
        },
      },
    },
  },
}));

const StyledNavLink = styled(NavLink)(({ theme }) => ({
  '&.active': {
    [`& .${treeItemClasses.root}`]: {
      [`& .${treeItemClasses.content}`]: {
        backgroundColor: `var(--tree-view-bg-color, ${theme.palette.action.selected})`,
        color: 'var(--tree-view-color)',
        fontWeight: theme.typography.fontWeightBold,
        '&.Mui-focused, &.Mui-selected': {
          backgroundColor: `var(--tree-view-bg-color, ${theme.palette.action.selected})`,
          color: 'var(--tree-view-color)',
          fontWeight: theme.typography.fontWeightBold,
        },
      },
    },
  },
  '&': {
    [`& .${treeItemClasses.root}`]: {
      [`& .${treeItemClasses.content}`]: {
        '&.Mui-focused, &.Mui-selected': {
          backgroundColor: 'inherit',
          color: 'inherit',
        },
        '&.Mui-selected:hover': {
          backgroundColor: theme.palette.action.hover,
        },
      },
    },
  },
}));

type Props = {
  /**
   * Injected by the documentation to work in an iframe.
   * You won't need it on your project.
   */
  window?: () => Window;
};

export default function Sidebar({ window }: Props) {
  const [mobileOpen, setMobileOpen] = useState(false);

  const handleDrawerToggle = () => {
    setMobileOpen(!mobileOpen);
  };

  const container = window !== undefined ? () => window().document.body : undefined;

  const drawer = (
    <Box>
      <CardMedia
        style={{ padding: '3rem', width: '12rem', margin: 'auto' }}
        component="img"
        image={logo}
        alt="Logo image"
      />
      <TreeView
        aria-label="admin"
        defaultExpanded={['2']}
        defaultCollapseIcon={<KeyboardArrowDownRounded />}
        defaultExpandIcon={<KeyboardArrowRightRounded />}
        defaultEndIcon={<div style={{ width: 24 }} />}
      >
        <StyledNavLink end to={`/`}>
          <StyledTreeItem
            nodeId="1"
            labelText="Tổng quan"
            labelIcon={() => <IonIcon icon={cubeOutline} />}
          />
        </StyledNavLink>
        <StyledTreeItem
          nodeId="2"
          labelText="Quản lý"
          labelIcon={() => <IonIcon icon={funnelOutline} />}
        >
          <CustomDivider />
          <StyledTreeItem
            nodeId="3"
            labelText="Đề tài"
            labelIcon={() => <IonIcon icon={copyOutline} />}
          >
            <StyledNavLink end to={`/admin/${TopicRoutes.TOPICS}/${TopicRoutes.REGISTRATION}`}>
              <StyledTreeItem
                nodeId="4"
                labelText="Danh sách đăng ký"
                labelIcon={() => <IonIcon icon={documentsOutline} />}
              />
            </StyledNavLink>
            <CustomDivider />
            <StyledNavLink end to={`/admin/${TopicRoutes.TOPICS}`}>
              <StyledTreeItem
                nodeId="5"
                labelText="Danh sách đề tài"
                labelIcon={() => <IonIcon icon={albumsOutline} />}
              />
            </StyledNavLink>
          </StyledTreeItem>
          <CustomDivider />
          <StyledNavLink end to={`/admin/${TopicRoutes.TOPICS}/${TopicRoutes.DRAFT}`}>
            <StyledTreeItem
              nodeId="6"
              labelText="Đăng ký"
              labelIcon={() => <IonIcon icon={brushOutline} />}
            />
          </StyledNavLink>
          <CustomDivider />
          <StyledNavLink end to={`/admin/${UserRoutes.USERS}`}>
            <StyledTreeItem
              nodeId="7"
              labelText="Tài khoản"
              labelIcon={() => <IonIcon icon={peopleOutline} />}
            />
          </StyledNavLink>
          <CustomDivider />
          <StyledNavLink end to={`/`}>
            <StyledTreeItem
              nodeId="8"
              labelText="Bài viết"
              labelIcon={() => <IonIcon icon={documentTextOutline} />}
            />
          </StyledNavLink>
          <CustomDivider />
          <StyledNavLink end to={`/admin/${TopicRoutes.TOPICS}/${TopicRoutes.RANKS}/${new Date().getFullYear()}`}>
            <StyledTreeItem
              nodeId="9"
              labelText="Hạng mục"
              labelIcon={() => <IonIcon icon={bookmarksOutline} />}
            />
          </StyledNavLink>
          <CustomDivider />
          <StyledNavLink end to={`/`}>
            <StyledTreeItem
              nodeId="10"
              labelText="Thông báo"
              labelIcon={() => <IonIcon icon={notificationsOutline} />}
            />
          </StyledNavLink>
          <CustomDivider />
        </StyledTreeItem>
        <StyledNavLink end to={`/`}>
          <StyledTreeItem
            nodeId="11"
            labelText="Cài đặt"
            labelIcon={() => <IonIcon icon={settingsOutline} />}
          />
        </StyledNavLink>
        <StyledNavLink end to={`/`}>
          <StyledTreeItem
            nodeId="12"
            labelText="Đăng xuất"
            labelIcon={() => <IonIcon icon={keyOutline} />}
          />
        </StyledNavLink>
      </TreeView>
    </Box>
  );

  return (
    <Box
      component="nav"
      sx={{
        height: '100vh',
        width: { sm: DRAWER_WIDTH },
        flexShrink: { sm: 0 },
        boxShadow: '3px -5px 10px 0px rgba(71,156,69,0.3)',
      }}
      aria-label="mailbox folders"
    >
      {/* The implementation can be swapped with js to avoid SEO duplication of links. */}
      <CustomDrawer
        container={container}
        variant="temporary"
        open={mobileOpen}
        onClose={handleDrawerToggle}
        ModalProps={{
          keepMounted: true, // Better open performance on mobile.
        }}
        sx={{
          display: { xs: 'block', sm: 'none' },
          '& .MuiDrawer-paper': { boxSizing: 'border-box', width: DRAWER_WIDTH },
        }}
      >
        {drawer}
      </CustomDrawer>
      <CustomDrawer variant="permanent" open sx={{ display: 'block' }}>
        <CustomScrollbar
          // eslint-disable-next-line tailwindcss/no-custom-classname
          className="custom-scrollbar"
          autoHide
          autoHideTimeout={1000}
          autoHideDuration={200}
        >
          {drawer}
        </CustomScrollbar>
      </CustomDrawer>
    </Box>
  );
}
